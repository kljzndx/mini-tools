import { GameMode, GameModes } from "./gameModes";
import { RandomMode, RandomModes } from "./randomModes";

class GameSettings{
  /**
   * @type {GameMode}
   */
  gameMode
  /**
   * @type {RandomMode}
   */
  randomMode
  /**
   * @type {number}
   */
  colsCount

  /**
   * @type {number}
   */
  selectTimes

  /**
   * @type {number}
   */
  questionCount

  /**
   * @type {boolean}
   */
  isShowCrosshair

  /**
   * @type {boolean}
   */
  isBlindMode

  /**
   * @type {boolean}
   */
  isShowCols

  /**
   * @param {GameMode} gameMode 
   * @param {RandomMode} randomMode 
   * @param {number} colsCount 
   * @param {number} selectTimes 
   * @param {number} questionCount 
   * @param {boolean} isShowCrosshair 
   * @param {boolean} isBlindMode 
   * @param {boolean} isShowCols 
   */
  constructor (gameMode, randomMode, colsCount, selectTimes, questionCount, isShowCrosshair, isBlindMode, isShowCols){
      this.gameMode=gameMode
      this.randomMode=randomMode
      this.colsCount=colsCount
      this.selectTimes=selectTimes
      this.questionCount=questionCount
      this.isShowCrosshair=isShowCrosshair
      this.isBlindMode=isBlindMode
      this.isShowCols=isShowCols
  }
}

export {
  GameSettings,
  GameModes,
  RandomModes,
}