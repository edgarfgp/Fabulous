namespace Fabulous

open System

type ArraySlice<'v> = (struct (uint16 * 'v array))

module ArraySlice =
    let inline toSpan (a: ArraySlice<'v>) =
        let struct (size, arr) = a
        Span(arr, 0, int size)

    let inline fromArrayOpt (arr: 'v [] option) : ArraySlice<'v> voption =
        match arr with
        | None -> ValueNone
        | Some arr -> ValueSome(uint16 arr.Length, arr)

    let inline fromArray (arr: 'v []) : ArraySlice<'v> voption =
        match arr.Length with
        | 0 -> ValueNone
        | size -> ValueSome(uint16 size, arr)

    let shiftByMut (slice: ArraySlice<'v> inref) (by: uint16) : 'v array =
        let struct (used, arr) = slice

        let used = int used
        let by = int by

        // noop if we don't have enough space
        if (used + by <= arr.Length) then
            for i = used + by - 1 downto int by do
                arr.[i] <- arr.[i - by]

        arr

//    let test () =
//        let slice = ArraySlice(2us, [| 1; 2; 0; 0 |])
//        let arr = shiftByMut &slice 2us
//        printfn "%A" arr
