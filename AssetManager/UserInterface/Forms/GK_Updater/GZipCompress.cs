using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Text;
namespace AssetManager.UserInterface.Forms.GK_Updater
{

	public class GZipCompress
	{

		private ProgressCounter Progress;
		public GZipCompress(ProgressCounter progress)
		{
			this.Progress = progress;
		}

		private void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
		{
			//Compress file name
			char[] chars = sRelativePath.ToCharArray();
			zipStream.Write(BitConverter.GetBytes(chars.Length), 0, 4);
			foreach (char c in chars) {
				zipStream.Write(BitConverter.GetBytes(c), 0, 2);
			}

			//Compress file content
			byte[] bytes = File.ReadAllBytes(Path.Combine(sDir, sRelativePath));
			zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
			zipStream.Write(bytes, 0, bytes.Length);
		}

		private bool DecompressFile(string sDir, GZipStream zipStream)
		{
			//Decompress file name
			byte[] bytes = new byte[4];
			int Readed = zipStream.Read(bytes, 0, 4);
			if (Readed < 4) {
				return false;
			}

			int iNameLen = BitConverter.ToInt32(bytes, 0);
			bytes = new byte[2];
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i <= iNameLen - 1; i++) {
				zipStream.Read(bytes, 0, 2);
				char c = BitConverter.ToChar(bytes, 0);
				sb.Append(c);
			}
			string sFileName = sb.ToString();

			//Decompress file content
			bytes = new byte[4];
			zipStream.Read(bytes, 0, 4);
			int iFileLen = BitConverter.ToInt32(bytes, 0);

			bytes = new byte[iFileLen];
			zipStream.Read(bytes, 0, bytes.Length);

			string sFilePath = Path.Combine(sDir, sFileName);
			string sFinalDir = Path.GetDirectoryName(sFilePath);
			if (!Directory.Exists(sFinalDir)) {
				Directory.CreateDirectory(sFinalDir);
			}

			using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 256000)) {
				outFile.Write(bytes, 0, iFileLen);
			}
			Progress.BytesMoved = iFileLen;
			return true;
		}

		public void CompressDirectory(string sInDir, string sOutFile)
		{
			string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
			int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;
			Progress.ResetProgress();
			Progress.BytesToTransfer = sFiles.Length - 1;
			FileStream outFile = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None, 256000);
			using (GZipStream str = new GZipStream(outFile, CompressionLevel.Optimal)) {
				foreach (string sFilePath in sFiles) {
					Progress.BytesMoved = 1;
					string sRelativePath = sFilePath.Substring(iDirLen);
					CompressFile(sInDir, sRelativePath, str);
				}
			}
		}

		public void DecompressToDirectory(string sCompressedFile, string sDir)
		{
			Progress.BytesToTransfer = GetGzOriginalFileSize(sCompressedFile);
			FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None, 256000);
			using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true)) {

				while (DecompressFile(sDir, zipStream)) {
				}
			}
		}

		/// <summary>
		/// Extracts the original filesize of the compressed file.
		/// </summary>
		/// <param name="fi">GZip file to handle</param>
		/// <returns>Size of the compressed file, when its decompressed.</returns>
		/// <remarks>More information at <a href="http://tools.ietf.org/html/rfc1952">http://tools.ietf.org/html/rfc1952</a> section 2.3</remarks>
		public static int GetGzOriginalFileSize(string fi)
		{
			return GetGzOriginalFileSize(new FileInfo(fi));
		}

		/// <summary>
		/// Extracts the original filesize of the compressed file.
		/// </summary>
		/// <param name="fi">GZip file to handle</param>
		/// <returns>Size of the compressed file, when its decompressed.</returns>
		/// <remarks>More information at <a href="http://tools.ietf.org/html/rfc1952">http://tools.ietf.org/html/rfc1952</a> section 2.3</remarks>
		public static int GetGzOriginalFileSize(FileInfo fi)
		{
			try {
				using (FileStream fs = fi.OpenRead()) {
					byte[] fh = new byte[3];
					fs.Read(fh, 0, 3);
					if (fh[0] == 31 && fh[1] == 139 && fh[2] == 8) {
						//If magic numbers are 31 and 139 and the deflation id is 8 then...
						byte[] ba = new byte[4];
						fs.Seek(-4, SeekOrigin.End);
						fs.Read(ba, 0, 4);
						return BitConverter.ToInt32(ba, 0);
					} else {
						return -1;
					}
				}
			} catch (Exception generatedExceptionName) {
				return -1;
			}
		}

	}
}
