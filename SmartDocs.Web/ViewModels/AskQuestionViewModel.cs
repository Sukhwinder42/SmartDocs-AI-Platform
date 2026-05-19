namespace SmartDocs.Web.ViewModels
{
    public class AskQuestionViewModel
    {
        public Guid DocumentId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public List<ChatMessageVm> ChatHistory { get; set; }
            = new List<ChatMessageVm>();
    }
}
