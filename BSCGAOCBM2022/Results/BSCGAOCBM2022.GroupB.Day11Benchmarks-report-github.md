``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |         Mean |      Error |        Ratio |   Gen0 | Allocated |    Alloc Ratio |
|------------- |-------------:|-----------:|-------------:|-------:|----------:|---------------:|
|  Auros_Part1 |     13.44 μs |   0.047 μs |     baseline |      - |         - |             NA |
| Caeden_Part1 |     32.54 μs |   0.117 μs | 2.42x slower | 0.9766 |    8232 B |             NA |
|   Eris_Part1 |     11.99 μs |   0.067 μs | 1.12x faster |      - |         - |             NA |
|              |              |            |              |        |           |                |
|  Auros_Part2 | 12,886.62 μs |  43.625 μs |     baseline |      - |       6 B |                |
| Caeden_Part2 | 15,651.74 μs |  65.313 μs | 1.21x slower |      - |    8518 B | 1,419.67x more |
|   Eris_Part2 | 11,606.28 μs | 111.333 μs | 1.11x faster |      - |       6 B |     1.00x more |
