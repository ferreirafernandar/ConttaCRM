namespace Arquitetura.Infraestrutura.Validator
{
    public class ValidationResult
    {
        public string ErrorMessage { get; private set; }
        public string MemberNames { get; private set; }

        public ValidationResult(string errorMessage, string memberNames)
        {
            this.ErrorMessage = errorMessage;
            this.MemberNames = memberNames;
        }
    }
}
