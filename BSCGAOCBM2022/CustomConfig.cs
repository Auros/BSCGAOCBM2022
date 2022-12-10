using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;

namespace BSCGAOCBM2022;

public class CustomConfig : ManualConfig
{
	public CustomConfig()
	{
		SummaryStyle = SummaryStyle.Default
			.WithRatioStyle(RatioStyle.Trend);

        HideColumns("Categories", "StdDev", "RatioSD");
	}
}