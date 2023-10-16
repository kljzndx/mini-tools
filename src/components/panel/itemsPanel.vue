<script setup>
import NumItem from "../templates/numItem.vue";
import { GameSettings } from '../../models/settings/gameSettings'
import { GameMode } from '../../models/settings/gameModes'
import { Point, RandomItemsWarper } from '../../models/settings/randomModes'
import { ref, watch } from "vue";

const props=defineProps({
    gameSettings:GameSettings,
    selectStatus:RandomItemsWarper,
})

const list=ref(getList(props.gameSettings.colsCount, props.gameSettings.gameMode))

/**
 * 
 * @param {number} colsCount 
 * @param {GameMode} gameMode 
 */
function getList(colsCount, gameMode) {
    const yList=[]
    for (let y = 0; y < colsCount; y++) {
        const xList=[]

        for (let x = 0; x < colsCount; x++) {
            const coreVal=gameMode.calculate(x+1, y+1)
            
            const element = {
                location:new Point(x, y),
                val:coreVal,
            }
            xList.push(element)
        }
        
        yList.push({
            id:y,
            items:xList,
        })
    }

    return yList
}

watch([()=>props.gameSettings.colsCount, ()=>props.gameSettings.gameMode], ([colsCount, gameMode])=>{
    list.value=getList(colsCount, gameMode)
})

function ItemFinished(text, slt){
    const ss=document.querySelectorAll('.answer-input')
    for (const item of ss) {
        if (item.value!=text){
            item.focus()
            break
        }
    }
    
    slt.isFinished=true
}

</script>
<template>
    <div>
        <ul>
            <li v-for="yItem of list" :key="yItem.id" class="flex">
                <NumItem v-for="(xItem, xId) of yItem.items" :key="xId" :location="xItem.location" :value="xItem.val" :select-status="props.selectStatus" :is-show-crosshair="props.gameSettings.isShowCrosshair" :is-blind-mode="props.gameSettings.isBlindMode" @finish="ItemFinished"/>
            </li>
        </ul>
    </div>
</template>