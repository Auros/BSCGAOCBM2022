``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |       Mean |   Error |        Ratio |    Gen0 |    Gen1 |    Gen2 | Allocated | Alloc Ratio |
|------------- |-----------:|--------:|-------------:|--------:|--------:|--------:|----------:|------------:|
|  Auros_Part1 |   375.3 μs | 1.49 μs |     baseline | 41.5039 | 41.5039 | 41.5039 | 364.75 KB |             |
| Caeden_Part1 |   226.8 μs | 0.84 μs | 1.66x faster | 38.3301 | 38.3301 | 38.3301 |  270.5 KB |  1.35x less |
|   Eris_Part1 |   435.5 μs | 1.00 μs | 1.16x slower | 41.5039 | 41.5039 | 41.5039 | 364.79 KB |  1.00x more |
|              |            |         |              |         |         |         |           |             |
|  Auros_Part2 |   544.7 μs | 1.72 μs |     baseline | 23.4375 |  6.8359 |       - | 200.25 KB |             |
| Caeden_Part2 |   380.1 μs | 0.61 μs | 1.43x faster |  4.3945 |       - |       - |  36.57 KB |  5.48x less |
|   Eris_Part2 | 1,046.0 μs | 2.49 μs | 1.92x slower | 23.4375 |  5.8594 |       - |  200.4 KB |  1.00x more |
