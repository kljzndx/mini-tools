class NumList{
    /** @type {number[]} */
    nums

    /** @type {string} */
    splitSymbol

    /** @type {number} */
    eachMoney

    constructor(nums, splitSymbols, eachMoney){
        this.nums=nums
        this.eachMoney=eachMoney

        if (splitSymbols.length==0){
            this.splitSymbol='-'
        } else {
            const random=Math.floor(Math.random()*splitSymbols.length)
            this.splitSymbol=splitSymbols[random]
        }
    }

    toString(){
        return `${this.nums.map(v=>v.toString().padStart(2, '0')).join(this.splitSymbol)}  各：${this.eachMoney}  总：${this.eachMoney*this.nums.length}`
    }
}

export {NumList}