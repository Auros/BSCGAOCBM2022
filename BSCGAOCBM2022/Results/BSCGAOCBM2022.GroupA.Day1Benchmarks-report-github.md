``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|       Method |      Mean |    Error |         Ratio |    Gen0 |   Gen1 | Allocated |   Alloc Ratio |
|------------- |----------:|---------:|--------------:|--------:|-------:|----------:|--------------:|
|  Auros_Part1 |  27.22 μs | 0.122 μs |      baseline |  1.2207 | 0.0305 |   10312 B |               |
| Caeden_Part1 | 131.69 μs | 0.819 μs |  4.84x slower | 17.5781 | 1.9531 |  147968 B |  14.349x more |
|   Eris_Part1 |  40.07 μs | 0.450 μs |  1.47x slower |       - |      - |      80 B | 128.900x less |
| Goobie_Part1 | 373.73 μs | 3.715 μs | 13.73x slower | 25.3906 | 2.9297 |  213880 B |  20.741x more |
|              |           |          |               |         |        |           |               |
|  Auros_Part2 |  35.03 μs | 0.154 μs |      baseline |  1.2207 |      - |   10312 B |               |
| Caeden_Part2 | 131.54 μs | 0.405 μs |  3.75x slower | 17.5781 | 1.9531 |  148016 B |   14.35x more |
|   Eris_Part2 |  40.39 μs | 0.128 μs |  1.15x slower |  0.3662 |      - |    3520 B |    2.93x less |
| Goobie_Part2 | 380.02 μs | 4.976 μs | 10.85x slower | 25.3906 | 2.9297 |  213960 B |   20.75x more |
