``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2364/21H2/November2021Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|     Method |           Mean |         Error |   Gen0 | Allocated |
|----------- |---------------:|--------------:|-------:|----------:|
| Eris_Part1 |       1.451 μs |     0.0054 μs | 0.0324 |     272 B |
|            |                |               |        |           |
| Eris_Part2 | 610,934.350 μs | 2,196.9131 μs |      - |     384 B |
