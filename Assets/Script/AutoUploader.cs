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
    // ���尡 �Ϸ�Ǹ� �� �Լ��� �ڵ����� ȣ��˴ϴ�.
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        UnityEngine.Debug.Log("���� �Ϸ�! ���� ����̺� ���ε带 �����մϴ�.");

        // ����� ������ ��θ� �����ɴϴ�.
        string buildDirectoryPath = Path.GetDirectoryName(pathToBuiltProject);

        // rclone�� ����Ͽ� ���� ������ ��°�� ���� ����̺꿡 ���ε��մϴ�.
        // ���� ������ �����մϴ�.
        string rcloneRemoteName = "meami"; // rclone config���� ������ ���� �̸�
        string rcloneDestPath = "Unity_Builds"; // ���� ����̺� �� ���ε��� ����

        // ���ε��� ���� �̸��� Ÿ�ӽ������� �߰��Ͽ� �ߺ��� ���մϴ�.
        string destFolderName = $"{Application.productName}_{DateTime.Now:yyyyMMdd_HHmmss}";
        string arguments = $"copy \"{buildDirectoryPath}\" \"{rcloneRemoteName}:{rcloneDestPath}/{destFolderName}\"";

        // rclone ���� ������ ��ü ��θ� �����մϴ�.
        // ����: C:\\rclone\\rclone.exe
        string rclonePath = Path.Combine(Application.dataPath, @"rclone-v1.71.0-windows-amd64/rclone.exe");

        if (!File.Exists(rclonePath))
        {
            UnityEngine.Debug.LogError("rclone.exe ������ ������ ��ο��� ã�� �� �����ϴ�. ��θ� Ȯ�����ּ���.");
            // ���� �߻� �� �ε��� ������ �ʵ��� ��ũ��Ʈ�� �����մϴ�.
            return;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = rclonePath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true, // ���� ��µ� ���𷺼�
            CreateNoWindow = true
        };

        try
        {
            Process process = Process.Start(startInfo);
            // rclone ���μ����� ����� ������ ��ٸ� ��, ����� �н��ϴ�.
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (process.ExitCode == 0)
            {
                UnityEngine.Debug.Log("���� ����̺� ���ε� ����!");
                DiscordBot.Create("https://discord.com/api/webhooks/1409483541007568947/zYWeWJu9wHmdEfKC_Rb9bEAe9EZIqqIiZuLmu9Jor8Vo4Vp7a9Wmv3VGVG_Ml3z2UAri")
                .WithEmbed
                    (
                    Embed.Create().WithURL("https://drive.google.com/drive/folders/1WaaRLNnnMTOT0L6i68ybcK2Gc20fkyre?usp=sharing").WithTitle("����˸�").WithDescription("���尡 ���������� �Ϸ�Ǿ����ϴ�.").WithColor(Color.green)
                    ).Send();
            }
            else
            {
                UnityEngine.Debug.LogError($"���� ����̺� ���ε� ����: rclone ���� �ڵ� {process.ExitCode}");
                UnityEngine.Debug.LogError($"rclone ���: {output}");
                UnityEngine.Debug.LogError($"rclone ����: {error}");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"���ε� �� ���� �߻�: {e.Message}");
        }
    }
}
#endif
