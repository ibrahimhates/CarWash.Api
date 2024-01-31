namespace CarWash.Core.Validation
{
    public class ValidationResultModel
    {
        public Dictionary<string, string> Errors { get; set; }
        public bool IsValid => Errors == null || !Errors.Any();
    }
}
