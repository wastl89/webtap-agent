using OpenTap;
using webtap_agent.Exceptions;
using webtap_agent.Models;

namespace webtap_agent.Services;

public class TestplanService(string basePath)
{
    private string _pathToTestplanFolder = basePath;
    private TestPlan? _testPlan;

    public TestPlan? TestPlan => _testPlan;


    public IEnumerable<string> GetTestplansInFolder() {
        //Find Files in Folder
        var files = Directory.GetFiles(_pathToTestplanFolder, "*.TapPlan", SearchOption.AllDirectories);
        
        files = files.Select(f => f.Replace(_pathToTestplanFolder+"/", "")).ToArray();
        files = files.Select(f => f.Replace(".TapPlan", "")).ToArray();
        return files;
    }

    public void LoadTestPlan(string name) {
        string path = _pathToTestplanFolder+"/"+name+".TapPlan";
        if (!File.Exists(path)) throw new NotFoundException("Testplan", path);

        _testPlan = TestPlan.Load(path);
    }


}