<script setup>
import { ref, watch } from "vue";
import { GameSettings } from "../../models/settings/gameSettings";
import { RandomItemsWarper } from "../../models/settings/randomModes";
import ItemsPanel from "./itemsPanel.vue";
import ColumnItem from "../templates/ColumnItem.vue";

const props=defineProps({
  gameSettings:GameSettings,
})

const selectStatus=ref(new RandomItemsWarper())

function getColsList(count) {

  const numsArray=[]

  for (let index = 0; index < count; index++) {
    numsArray.push({id:index, val:index+1})
  }
  return numsArray
}

const colsList=ref(getColsList(props.gameSettings.colsCount))
const rowsList=ref(getColsList(props.gameSettings.colsCount))

watch([()=>props.gameSettings.colsCount, ()=>props.gameSettings.gameMode], ([colsCount, gameMode])=>{
  colsList.value=getColsList(colsCount)
  rowsList.value=getColsList(colsCount)
  selectStatus.value=new RandomItemsWarper()
})

watch([()=>props.gameSettings.randomMode, ()=>props.gameSettings.selectTimes,()=>props.gameSettings.questionCount], ([randomMode, selectTimes, questionCount])=>{
  beginSelect()
})

function beginSelect(){
  const source=props.gameSettings.randomMode.select(props.gameSettings.colsCount, props.gameSettings.selectTimes)
  source.randomWritable(props.gameSettings.questionCount)
  selectStatus.value=source
}

</script>

<template>
  <div>
    <div class="w-min grid grid-cols-[auto_1fr] grid-rows-[auto_1fr_auto]">
      <div class="shortTitle">
        <ColumnItem :text="props.gameSettings.gameMode.shortTitle" is-show-cols/>
      </div>
      <div class="col-start-2 flex ">
        <ColumnItem v-for="item of colsList" :key="item.id" :id="item.id" :text="item.val.toString()" :select-status="selectStatus" orientation="x" :is-blind-mode="props.gameSettings.isBlindMode" :is-show-cols="gameSettings.isShowCols"/>
      </div>
      <div class="row-start-2 flex flex-col">
        <ColumnItem v-for="item of rowsList" :key="item.id" :id="item.id" :text="item.val.toString()" :select-status="selectStatus" orientation="y" :is-blind-mode="props.gameSettings.isBlindMode" :is-show-cols="gameSettings.isShowCols"/>
      </div>
      <div class="row-start-2 col-start-2">
        <ItemsPanel :game-settings="props.gameSettings" :select-status="selectStatus"/>
      </div>

      <button class="row-start-3 col-start-1 col-span-2 mx-auto my-3
                     px-3 py-2 rounded-lg bg-violet-500 hover:bg-violet-400 active:bg-violet-600
                     text-white text-lg font-bold
                      " @click="beginSelect()">随机选择</button>
    </div>
  </div>
</template>

<style scoped>
</style>
