namespace BigBash.Models
{
    public class GeminiResponseModel
    {
        public List<Candidate> Candidates { get; set; }
        public UsageMetadata UsageMetadata { get; set; }
        public string ModelVersion { get; set; }
    }

    public class Candidate
    {
        public Content Content { get; set; }
        public string FinishReason { get; set; }
        public double AvgLogprobs { get; set; }
    }

    public class Content
    {
        public List<Part> Parts { get; set; }
        public string Role { get; set; }
    }

    public class Part
    {
        public string Text { get; set; }
    }

    public class UsageMetadata
    {
        public int PromptTokenCount { get; set; }
        public int CandidatesTokenCount { get; set; }
        public int TotalTokenCount { get; set; }
        public List<TokenDetail> PromptTokensDetails { get; set; }
        public List<TokenDetail> CandidatesTokensDetails { get; set; }
    }

    public class TokenDetail
    {
        public string Modality { get; set; }
        public int TokenCount { get; set; }
    }
}