#if UNITY_EDITOR
using NKStudio.Discord;
using NKStudio.Discord.Module;
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[InitializeOnLoad]
public class AutoUploader
{
    // 빌드가 완료되면 이 함수가 자동으로 호출됩니다.
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        UnityEngine.Debug.Log("빌드 완료! 구글 드라이브 업로드를 시작합니다.");

        // 빌드된 폴더의 경로를 가져옵니다.
        string buildDirectoryPath = Path.GetDirectoryName(pathToBuiltProject);

        // rclone을 사용하여 빌드 폴더를 통째로 구글 드라이브에 업로드합니다.
        // 압축 과정은 생략합니다.
        string rcloneRemoteName = "meami"; // rclone config에서 설정한 원격 이름
        string rcloneDestPath = "Unity_Builds"; // 구글 드라이브 내 업로드할 폴더

        // 업로드할 폴더 이름에 타임스탬프를 추가하여 중복을 피합니다.
        string destFolderName = $"{Application.productName}_{DateTime.Now:yyyyMMdd_HHmmss}";
        string arguments = $"copy \"{buildDirectoryPath}\" \"{rcloneRemoteName}:{rcloneDestPath}/{destFolderName}\"";

        // rclone 실행 파일의 전체 경로를 지정합니다.
        // 예시: C:\\rclone\\rclone.exe
        string rclonePath = Path.Combine(Application.dataPath, @"rclone-v1.71.0-windows-amd64/rclone.exe");

        if (!File.Exists(rclonePath))
        {
            UnityEngine.Debug.LogError("rclone.exe 파일을 지정된 경로에서 찾을 수 없습니다. 경로를 확인해주세요.");
            // 오류 발생 후 로딩이 멈추지 않도록 스크립트를 종료합니다.
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = rclonePath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true, // 에러 출력도 리디렉션
            CreateNoWindow = true
        };

        try
        {
            Process process = Process.Start(startInfo);
            // rclone 프로세스가 종료될 때까지 기다린 후, 출력을 읽습니다.
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (process.ExitCode == 0)
            {
                UnityEngine.Debug.Log("구글 드라이브 업로드 성공!");
                DiscordBot.Create("https://discord.com/api/webhooks/1409483541007568947/zYWeWJu9wHmdEfKC_Rb9bEAe9EZIqqIiZuLmu9Jor8Vo4Vp7a9Wmv3VGVG_Ml3z2UAri")
                .WithEmbed
                    (
                    Embed.Create().WithURL("https://drive.google.com/drive/folders/1WaaRLNnnMTOT0L6i68ybcK2Gc20fkyre?usp=sharing").WithTitle("빌드알림").WithDescription("빌드가 성공적으로 완료되었습니다.").WithColor(Color.green)
                    ).Send();
            }
            else
            {
                UnityEngine.Debug.LogError($"구글 드라이브 업로드 실패: rclone 종료 코드 {process.ExitCode}");
                UnityEngine.Debug.LogError($"rclone 출력: {output}");
                UnityEngine.Debug.LogError($"rclone 에러: {error}");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"업로드 중 오류 발생: {e.Message}");
        }
    }
}
#endif
