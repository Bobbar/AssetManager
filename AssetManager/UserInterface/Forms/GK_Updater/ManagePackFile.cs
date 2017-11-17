using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace AssetManager.UserInterface.Forms.GK_Updater
{

    public class ManagePackFile
    {
        public ProgressCounter Progress;

        public string Status = "";
        public ManagePackFile()
        {
            Progress = new ProgressCounter();
        }

        /// <summary>
        /// Creates and cleans the pack file directories then downloads a new pack file from the server location.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DownloadPack()
        {
            if (!Directory.Exists(Paths.GKPackFileFDir))
            {
                Directory.CreateDirectory(Paths.GKPackFileFDir);
            }

            if (Directory.Exists(Paths.GKExtractDir))
            {
                Directory.Delete(Paths.GKExtractDir, true);
            }

            if (File.Exists(Paths.GKPackFileFullPath))
            {
                File.Delete(Paths.GKPackFileFullPath);
            }
            Progress = new ProgressCounter();
            return await CopyPackFile(Paths.GKRemotePackFilePath, Paths.GKPackFileFullPath);
        }

        /// <summary>
        /// Verifies directory structure, checks if pack file is present, then compares local and remote hashes of the pack file.
        ///
        /// Returns False if directory or file is missing, or if the hashes mismatch.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> VerifyPackFile()
        {
            try
            {
                if (!Directory.Exists(Paths.GKPackFileFDir))
                {
                    return false;
                }
                if (!Directory.Exists(Paths.GKExtractDir))
                {
                    return false;
                }
                if (!File.Exists(Paths.GKPackFileFullPath))
                {
                    return false;

                }
                else
                {
                    string LocalHash = await Task.Run(() => { return SecurityTools.GetMD5OfFile(Paths.GKPackFileFullPath); });

                    string RemoteHash = await Task.Run(() => { return GetRemoteHash(); });

                    if (LocalHash == RemoteHash)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
        }

        /// <summary>
        /// Returns the contents of the hash text file located in <see cref="Paths.GKRemotePackFileDir"/>
        /// </summary>
        /// <returns></returns>
        private string GetRemoteHash()
        {
            using (StreamReader sr = new StreamReader(Paths.GKRemotePackFileDir + Paths.GKPackHashName))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Copies a single file to the <paramref name="dest"/> path.
        /// </summary>
        /// <param name="source"></param>
        /// Path of source file.
        /// <param name="dest"></param>
        /// Path of destination.
        /// <returns></returns>
        public async Task<bool> CopyPackFile(string source, string dest)
        {
            if (File.Exists(dest))
            {
                File.Delete(dest);
            }
            return await Task.Run(() =>
            {
                try
                {
                    CopyFile(source, dest);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Performs a buffered file stream transfer.
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Dest"></param>
        private void CopyFile(string Source, string Dest)
        {
            int BufferSize = 256000;
            byte[] buffer = new byte[BufferSize];
            int bytesIn = 1;
            FileInfo CurrentFile = new FileInfo(Source);
            Progress.ResetProgress();
            using (System.IO.FileStream fStream = CurrentFile.OpenRead())
            using (FileStream destFile = new FileStream(Dest, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, BufferSize, FileOptions.None))
            {
                Progress.BytesToTransfer = (int)fStream.Length;
                while (!(bytesIn < 1))
                {
                    bytesIn = fStream.Read(buffer, 0, BufferSize);
                    if (bytesIn > 0)
                    {
                        destFile.Write(buffer, 0, bytesIn);
                        Progress.BytesMoved = bytesIn;
                    }
                }
            }
        }

        /// <summary>
        /// Compresses the local Gatekeeper directory into a new pack file.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> PackGKDir()
        {
            try
            {
                Progress = new ProgressCounter();
                GZipCompress CompDir = new GZipCompress(Progress);
                if (!Directory.Exists(Paths.GKPackFileFDir))
                {
                    Directory.CreateDirectory(Paths.GKPackFileFDir);
                }

                if (File.Exists(Paths.GKPackFileFullPath))
                {
                    File.Delete(Paths.GKPackFileFullPath);
                }
                await Task.Run(() => { CompDir.CompressDirectory(Paths.GKInstallDir, Paths.GKPackFileFullPath); });
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
        }

        /// <summary>
        /// Decompresses the pack file into a local working directory.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> UnPackGKDir()
        {
            try
            {
                Status = "Unpacking....";
                Progress = new ProgressCounter();
                GZipCompress CompDir = new GZipCompress(Progress);
                await Task.Run(() => { CompDir.DecompressToDirectory(Paths.GKPackFileFullPath, Paths.GKExtractDir); });
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
        }

        /// <summary>
        /// Copies the pack file and hash file to the server directory.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> UploadPackFiles()
        {
            bool Done = false;
            Status = "Uploading Pack File...";
            Progress = new ProgressCounter();
            Done = await CopyPackFile(Paths.GKPackFileFullPath, Paths.GKRemotePackFilePath);

            Status = "Uploading Hash File...";
            Progress = new ProgressCounter();
            Done = await CopyPackFile(Paths.GKPackFileFDir + Paths.GKPackHashName, Paths.GKRemotePackFileDir + Paths.GKPackHashName);
            return Done;
        }

        /// <summary>
        /// Verifies the local pack file and downloads a new one if needed.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ProcessPackFile()
        {
            bool PackFileOK = false;
            Status = "Verifying Pack File...";
            if (await VerifyPackFile())
            {
                PackFileOK = await UnPackGKDir();
            }
            else
            {
                Status = "Downloading Pack File...";
                if (await DownloadPack())
                {
                    PackFileOK = await UnPackGKDir();
                }
            }
            if (PackFileOK)
            {
                Status = "Done.";
                await Task.Run(() => { Thread.Sleep(1000); });
                return true;
            }
            else
            {
                Status = "ERROR!";
                return false;
            }

        }

        /// <summary>
        /// Creates a new pack file and hash file and copies them to the server location.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateNewPackFile()
        {
            try
            {
                bool OK = false;
                Status = "Creating Pack File...";
                Progress = new ProgressCounter();
                OK = await PackGKDir();

                Status = "Generating Hash...";
                OK = await CreateHashFile();

                OK = await UploadPackFiles();

                if (OK)
                {
                    Status = "Done.";
                }
                else
                {
                    Status = "Something went wrong...";
                }
                return OK;
            }
            catch (Exception ex)
            {
                Status = "ERROR!";
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
        }

        /// <summary>
        /// Creates a text file containing the hash string of the pack file.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CreateHashFile()
        {
            if (File.Exists(Paths.GKPackFileFDir + Paths.GKPackHashName))
            {
                File.Delete(Paths.GKPackFileFDir + Paths.GKPackHashName);
            }
            object Hash = await Task.Run(() => { return SecurityTools.GetMD5OfFile(Paths.GKPackFileFullPath); });
            using (StreamWriter sw = File.CreateText(Paths.GKPackFileFDir + Paths.GKPackHashName))
            {
                sw.Write(Hash);
            }
            return true;
        }

    }
}
