﻿/// Provides a model of stock focused on the fact that bins stored in the Storage Machine can be nested in each other,
/// in addition to holding products.
module StorageMachine.Repacking.BinTree

open StorageMachine
open Common

/// Multiple bins can be nested in each other, this forming a "tree" of bins.
type BinTree =
    /// A bin can contain zero or more other (nested) bins and products.
    | Bin of BinIdentifier * List<BinTree>
    /// A product is represented by its part number.
    | Product of PartNumber
    | PackagedProduct of PartNumber

/// Determines how many products are contained in all bins of the given bin tree.
let rec productCount binTree =
    match binTree with
    | Bin (_, productsOrBins) -> List.sumBy productCount productsOrBins
    | Product _ -> 1
    | PackagedProduct _ -> 1

let rec packageProducts (binTree: BinTree) : BinTree =
    match binTree with
    | Bin (id, productsOrBins) -> Bin(id, List.map packageProducts productsOrBins)
    | Product partNumber -> PackagedProduct(partNumber)
    | PackagedProduct _ as alreadyPackaged -> alreadyPackaged