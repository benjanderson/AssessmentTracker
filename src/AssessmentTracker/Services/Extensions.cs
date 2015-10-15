namespace AssessmentTracker.Services
{
	using System.IO;

	using Microsoft.AspNet.Http;

	public static class Extensions
    {
		public static byte[] ReadFile(this IFormFile file)
		{
			byte[] bytes;
			using (var memstream = new MemoryStream())
			{
				file.OpenReadStream().CopyTo(memstream);
				bytes = memstream.ToArray();
			}
			return bytes;
		}
	}
}
