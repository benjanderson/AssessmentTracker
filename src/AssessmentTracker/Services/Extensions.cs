namespace AssessmentTracker.Services
{
	using System.IO;
	using System.Text.RegularExpressions;

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

		public static string ToSentenceCase(this string str)
		{
			return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
		}
	}
}
