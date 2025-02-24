namespace webtap_agent.Models;

public class TestPlanLoadDto {
    public string? Name;
    public bool IsOpen;
    public IEnumerable<string>? ExternalParameters;
}