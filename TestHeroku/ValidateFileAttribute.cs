using System.ComponentModel.DataAnnotations;

namespace IntelliCRMAPIService.Attribute
{
    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int MaxContentLength = (int)(1024 * 1024 * 0.5);
            string[] AllowedFileExtensions = new string[] { ".xlsx" };
            var file = value as IFormFile;
            if (file == null)
                return false;
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Upload File Type: " + string.Join(", ", AllowedFileExtensions);
                return false;
            }
            else if (file.Length > MaxContentLength)
            {
                ErrorMessage = "The Size of file is too large : " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }
    }
}
