
class GameMode{
    /**
     * @type {number}
     */
    id

    /**
     * @type {string}
     */
    shortTitle

    /**
     * @type {string}
     */
    title

    /**
     * @callback calculateCallback
     * @param {number} x
     * @param {number} y
     * @returns {number}
     */

    /**
     * @param {number} id 
     * @param {string} shortTitle 
     * @param {string} title 
     * @param {calculateCallback} calculate 
     */
    constructor (id, shortTitle, title, calculate){
        this.id=id
        this.shortTitle=shortTitle
        this.title=title

        /**
         * @type {calculateCallback} 
         */
        this.calculate=calculate
    }
}

// const GameModes={
//   plus:new GameMode(1, '+', '加法', (x,y)=>x+y),
//   minus:new GameMode(2, '-', '减法', (x,y)=>x-y),
//   multiply:new GameMode(3, '*', '乘法', (x,y)=>x*y),
//   divide:new GameMode(4, '/', '除法', (x,y)=>x/y),
// }

const GameModes=[
    new GameMode(1, '+', '加法', (x,y)=>x+y),
    new GameMode(2, '-', '减法', (x,y)=>x-y),
    new GameMode(3, '*', '乘法', (x,y)=>x*y),
    new GameMode(4, '/', '除法', (x,y)=>parseFloat((x/y).toFixed(2))),
]

export {
    GameMode,
    GameModes
}