﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Core.GoogleAPI
{
	/// <summary>
	/// Google Drive API
	/// </summary>
	public static class DriveAPI
	{
		/// <summary>
		/// The scopes
		/// </summary>
		private static string[] Scopes = { DriveService.Scope.Drive };

		/// <summary>
		/// The application name
		/// </summary>
		private static string ApplicationName = "Upload";

		/// <summary>
		/// Uploads the file.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		public static string UploadImage(MemoryStream stream, string fileName)
		{
			var service = Init();

			return Upload(stream, service, fileName);
		}

		/// <summary>
		/// Initializes drive service.
		/// </summary>
		/// <returns>Drive Service</returns>
		private static DriveService Init()
		{
			UserCredential credential;
			credential = GetCredentials();
			// Create Drive API service.
			var service = new DriveService(new BaseClientService.Initializer()

			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});
			
			return service;
		}

		/// <summary>
		/// Uploads the specified file to google drive.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="service">The service.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>File identifier</returns>
		private static string Upload(MemoryStream stream, DriveService service, string fileName)
		{
			var fileMetadata = new Google.Apis.Drive.v3.Data.File();
			fileMetadata.Name = fileName;
			fileMetadata.MimeType = "image/jpeg";
			fileMetadata.Parents = new List<string> { "1GW0sqrlpAwAwaIG439k7SRQ0N7qjUHPj" };	
			FilesResource.CreateMediaUpload request;
			request = service.Files.Create(fileMetadata, stream, "image/jpeg");
			request.Fields = "id";
			request.Upload();

			var file = request.ResponseBody;

			return file.Id;
		}

		/// <summary>
		/// Gets the credentials.
		/// </summary>
		/// <returns>User credentials</returns>
		private static UserCredential GetCredentials()
		{
			UserCredential credential;
			var path = AppContext.BaseDirectory;
			path = path.Remove(path.IndexOf("Admin\\"));
			path += "Core\\GoogleAPI";
			using (var stream = new FileStream(path + "\\client_secret.json", FileMode.Open, FileAccess.Read))
			{
				string credPath = path;
				credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					Scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
			}

			return credential;
		}

		/// <summary>
		/// Deletes the file.
		/// </summary>
		/// <param name="fileId">The file identifier.</param>
		public static void DeleteFile(string fileId)
		{
			var service = Init();

			try
			{
				service.Files.Delete(fileId).Execute();
			}
			catch (Exception)
			{
			}
		}
	}
}
