``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |       Mean |   Error |        Ratio |   Gen0 | Allocated | Alloc Ratio |
|------------- |-----------:|--------:|-------------:|-------:|----------:|------------:|
|  Auros_Part1 |   344.5 ns | 2.34 ns |     baseline |      - |         - |          NA |
| Caeden_Part1 |   445.7 ns | 0.60 ns | 1.29x slower |      - |         - |          NA |
|              |            |         |              |        |           |             |
|  Auros_Part2 |   782.8 ns | 5.44 ns |     baseline | 0.0620 |     520 B |             |
| Caeden_Part2 | 1,080.1 ns | 6.26 ns | 1.38x slower | 0.1984 |    1664 B |  3.20x more |
