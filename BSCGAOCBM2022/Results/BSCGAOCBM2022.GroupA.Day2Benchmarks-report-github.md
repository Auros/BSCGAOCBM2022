``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |     Mean |    Error |        Ratio |   Gen0 | Allocated |     Alloc Ratio |
|------------- |---------:|---------:|-------------:|-------:|----------:|----------------:|
|  Auros_Part1 | 66.43 μs | 0.372 μs |     baseline | 7.0801 |   60192 B |                 |
| Caeden_Part1 | 23.16 μs | 0.084 μs | 2.87x faster |      - |         - |              NA |
|   Eris_Part1 | 26.37 μs | 0.089 μs | 2.52x faster |      - |      32 B | 1,881.000x less |
| Goobie_Part1 | 20.03 μs | 0.073 μs | 3.32x faster |      - |         - |              NA |
|              |          |          |              |        |           |                 |
|  Auros_Part2 | 71.56 μs | 0.197 μs |     baseline | 7.0801 |   60192 B |                 |
| Caeden_Part2 | 23.16 μs | 0.071 μs | 3.09x faster |      - |         - |              NA |
|   Eris_Part2 | 25.85 μs | 0.064 μs | 2.77x faster |      - |      32 B | 1,881.000x less |
| Goobie_Part2 | 19.85 μs | 0.122 μs | 3.61x faster |      - |         - |              NA |
