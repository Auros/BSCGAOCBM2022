``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |       Mean |     Error |         Ratio |     Gen0 | Allocated | Alloc Ratio |
|------------- |-----------:|----------:|--------------:|---------:|----------:|------------:|
|  Auros_Part1 | 153.896 μs | 0.3262 μs |      baseline |  61.7676 |  516824 B |             |
| Caeden_Part1 |   4.974 μs | 0.0609 μs | 31.01x faster |        - |         - |          NA |
|   Eris_Part1 |   6.809 μs | 0.0397 μs | 22.60x faster |        - |         - |          NA |
|              |            |           |               |          |           |             |
|  Auros_Part2 | 507.270 μs | 2.1913 μs |      baseline | 125.9766 | 1058120 B |             |
| Caeden_Part2 |  17.890 μs | 0.3212 μs | 28.35x faster |        - |         - |          NA |
|   Eris_Part2 |  18.548 μs | 0.1952 μs | 27.35x faster |        - |         - |          NA |
