namespace Martiello.Domain.UseCase
{
    public class PresenterOptions
    {

        public PresenterOptions()
        {
            WrapResult = false;
            ResultKeyDescription = "Result";
            ErrorKeyDescription = "Errors";
            ErrorCodeKeyDescription = "ErrorCode";
        }
        public bool WrapResult { get; set; }
        public string ResultKeyDescription { get; set; }
        public string ErrorKeyDescription { get; set; }
        public string ErrorCodeKeyDescription { get; set; }
    }
}
