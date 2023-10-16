function getRandomId(count) {
    return Math.floor(Math.random()*count)
}

class Point{
    /**
     * @type {number}
     */
    x
    /**
     * @type {number}
     */
    y

    /**
     * @param {number} x 
     * @param {number} y 
     */
    constructor(x, y) {
        this.x = x
        this.y = y
    }
    
    
    /**
     * @param {object} other 
     * @param {number} other.x 
     * @param {number} other.y 
     * 
     */
    equals({x, y}){
        return this.x==x&&this.y==y
    }
}

class RandomItem {
    /**
     * @type {Point}
     */
    point

    isFinished = false
    canWrite = false

    /**
     * @param {Point} point
     */
    constructor(point) {
        this.point = point
    }
}

class RandomItemsWarper{
    /**
     * @type {RandomItem[]}
     */
    items

    /**
     * @param  {RandomItem[]} items 
     */
    constructor(items=[]){
        this.items=items
    }

    /**
     * @param {number} times
     */
    randomWritable(times){
        if (times<0){
            this.items.forEach(it=>it.canWrite=true)
            return
        }

        const tmsArr=[]
        let fallTimes=0
        while (tmsArr.length<times&&fallTimes<100) {
            fallTimes++
            const tm=getRandomId(this.items.length)

            if (tmsArr.includes(tm))
                continue

            tmsArr.push(tm)
            fallTimes=0
        }

        for (const id of tmsArr) {
            this.items[id].canWrite=true
        }
    }
}

class RandomMode {
    /**
     * @type {number}
     */
    id

    /**
     * @type {string}
     */
    title

    /**
     * @type {number}
     */
    minColsCount

    /**
     * @callback selectCoreCallback
     * @param {number} colsCount
     * @param {Point} point
     * @param {Point[]} allPoints
     * @returns {RandomItem[] | null}
     */

    /**
     * 
     * @param {number} id 
     * @param {number} minColsCount 
     * @param {string} title 
     * @param {selectCoreCallback} selectCore
     */
    constructor(id, minColsCount, title, selectCore) {
        this.id=id

        this.minColsCount=minColsCount
        this.title = title

        /**
         * @param {number} colsCount 
         * @param {number} execTimes 
         */
        this.select = (colsCount, execTimes) => {
            if (colsCount<this.minColsCount)
                return new RandomItemsWarper()

            /**
             * @type {Point[]}
             */
            const pointList = []
            /**
             * @type {RandomItem[]}
             */
            const itemList=[]

            let fallTimes=0

            while (pointList.length<execTimes&&fallTimes<100) {
                fallTimes++

                const point=new Point(getRandomId(colsCount), getRandomId(colsCount))
                if(pointList.find(it=>it.equals(point))!=undefined)
                    continue
                
                const items=selectCore(colsCount, point, pointList)
                if (items==null)
                    continue

                pointList.push(point)
                items.forEach(it=>{
                    if (itemList.find(the=>the.point.equals(it.point))==undefined)
                        itemList.push(it)
                })

                fallTimes=0
            }
            return new RandomItemsWarper(itemList)
        }
    }

}

const RandomModes=[
    new RandomMode(1, 1, '单点式', (colsCount, point, allPoints)=>{
        /**
         * @type {RandomItem[]}
         */
        const list=[]
        list.push(new RandomItem(point))
        return list

    }),

    new RandomMode(2, 3, '九宫格', (colsCount, point)=>{
        if (point.x==0||point.y==0||
            point.x==colsCount-1||point.y==colsCount-1)
            return null

        /**
         * @type {RandomItem[]}
         */
        const result=[]

        for (let yStart = point.y-1; yStart < point.y+2; yStart++) {
            for (let xStart = point.x-1; xStart < point.x+2; xStart++) {
                result.push(new RandomItem(new Point(xStart, yStart)))
            }
        }
        
        return result
    }),
]

// const RandomModes={
//     dot:new RandomMode(1, 1, '单点式', (colsCount, point, allPoints)=>{
//         /**
//          * @type {RandomItem[]}
//          */
//         const list=[]
//         list.push(new RandomItem(point))
//         return list

//     }),

//     box:new RandomMode(2, 3, '九宫格', (colsCount, point)=>{
//         if (point.x==0||point.y==0||
//             point.x==colsCount-1||point.y==colsCount-1)
//             return null

//         /**
//          * @type {RandomItem[]}
//          */
//         const result=[]

//         for (let yStart = point.y-1; yStart < point.y+2; yStart++) {
//             for (let xStart = point.x-1; xStart < point.x+2; xStart++) {
//                 result.push(new RandomItem(new Point(xStart, yStart)))
//             }
//         }
        
//         return result
//     }),
// }

export {
    Point,
    RandomItem,
    RandomItemsWarper,
    RandomMode,
    RandomModes,
}