# CrawfisSoftware.BasicGraphs

Small, experimental graph library used to explore alternative implementations of `IGraph` (and indexed variants) compared to the `IGraph` implementation used in the `Grid` project.

The intent of this repo is to keep the *graph query/traversal surface* (`IGraph` / `IIndexedGraph`) small and algorithm-friendly, while separating *construction and mutation* concerns into builder-style types. This makes it easier to reuse the same graph algorithms across multiple backing representations.

## What this repo contains

### Core interfaces (read-only usage)
- `IGraph<N,E>`: Graph API based on *node labels* (`N`) and *edge labels* (`E`). Optimized for algorithms that work with arbitrary node types.
- `IEdge<N,E>`: Edge view with `From`, `To`, and `Value`.

### Mutable label-based graph
- `Graph<N,E>`: A simple, mutable implementation of `IGraph<N,E>`.
  - Nodes are added via `AddNode(N)`.
  - Edges are added via `AddEdge(from, to, edgeLabel, undirected: true)`.
  - Intended as a straightforward reference implementation.

### Wrapper over a plain adjacency list
- `SimpleGraph`: An `IGraph<int,int>` wrapper over an external adjacency list (`IList<ICollection<int>>`).
  - This intentionally demonstrates a minimal backing store.
  - Does not support `Parents(...)` or `InEdges(...)` (throws) because the representation does not store that information.

## Indexed graphs (algorithm-friendly)
Many graph algorithms are simpler and faster when nodes are represented by stable integer indices. This repo provides an indexed graph abstraction:

- `IIndexedGraph<N,E>`: Read-only API where nodes are `int` indices, with optional node labels (`N`) and edge labels (`E`).
- `IIndexedEdge<E>` / `IndexedEdge<E>`: Edge view using `From`, `To` indices.

Implementations:
- `AdjacencyListIndexedGraph<N,E>`: Immutable-ish adjacency-list based indexed graph. Optionally treats the graph as undirected.
- `CompleteIndexedGraph<N,E>`: Lightweight complete graph backed by functions (`Func<int,N>` for nodes, `Func<int,int,E>` for edges) rather than stored adjacency.

## Construction (separate from `IGraph` / `IIndexedGraph`)

- `GraphBuilder<N,E>`: Incremental builder that collects nodes and edges into adjacency lists, then produces an `IIndexedGraph<N,E>` via `GetGraph()`.
  - This is the main example of keeping “build/change” out of the query interface.

## Notes / non-goals
- This repo is deliberately small and focused on representation experiments.
- It is *not* intended to be a full-featured graph framework (no algorithm suite here).
- API surface is oriented toward being consumed by the separate “Graph algorithms” repo.

## Build

```powershell
dotnet build
```
