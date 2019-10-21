# Performance and optimization

## Speeding up ground model comparisons

Initially the ground model encoded both props and tiles. This caused large hashing overheads since the solver needs to check the map for equality whenever hashing.
This was improved by separating the props (can move) from the map itself (static).

## Speeding up the clone method

With comparisons out of the way (for now), cloning represents almost 100% of processing overheads. So we'd like to provide our own. This works like a charm. Cloning via serialization may be slow, but another advantage when cloning manually is that we don't need a deep clone - if we already know that certain data isn't going to change, we just carry over by reference.

Speeding up the clone method relegated cloning to something like 1% of processing overheads.

## Back to hashing and equality

At this point the ground model comparison is 50% of planning cost, courtesy `SequenceEqual`. Can this be improved? Getting to the gritty details, we get this to 12%; notes:
- Arrays vs List
- If you know the type, don't use a generic equality comparer
- Avoid pattern matching (which btw may trigger GC)

At this point the situation is like this:
HashSet.Add = 35%
SentinelModel.Equals = 18%
GroundModel.Equals = 7%
The number of calls to Equals though is quite impressive - 40k, for 90 calls to HashSet.Add!
Bad hashcode?