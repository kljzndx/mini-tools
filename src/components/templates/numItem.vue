<script setup>
import { computed, ref, watch } from "vue";
import { Point, RandomItem, RandomItemsWarper } from "../../models/settings/randomModes";

const props = defineProps({
    location: Point,
    value: Number,
    selectStatus: RandomItemsWarper,
    isShowCrosshair: Boolean,
    isBlindMode:Boolean,
})

const ev= defineEmits({
    /**
     * 
     * @param {String} text
     * @param {RandomItem} slt 
     */
    finish:(text, slt)=>{
        return (text instanceof String) && (slt instanceof RandomItem)
    },
})

const isSelectEnabled=computed(()=>{
    return props.selectStatus != undefined
})

const selectItem=computed(()=>{
    if (isSelectEnabled.value) {
        for (const item of props.selectStatus.items) {
            if (item.point.equals(props.location))
                return item
        }
    }
    return null
})

const isSelected = computed(()=>{
    if (selectItem.value!=null)
        return true
    
    if (isSelectEnabled.value) {
        for (const item of props.selectStatus.items) {
            if (props.isShowCrosshair&&(item.point.x == props.location.x || item.point.y == props.location.y))
                return true
        }
    }

    return false
})

const canWrite=computed(()=>{
        return selectItem.value!=null&&selectItem.value.canWrite&&selectItem.value.isFinished==false
})

const isShowText=computed(()=>{
    if(canWrite.value)
        return false

    if (props.isBlindMode&&selectItem.value==null&&!isSelected.value)
        return false

    return true
})

const isFinished=computed(()=>{
    if (selectItem.value!=null)
    {
        if (selectItem.value.canWrite)
            return selectItem.value.isFinished
        else {
            const array = props.selectStatus.items.filter(i=>i.canWrite)
            for (const item of array) {
                if (item.isFinished==false)
                    return false
            }
            return true
        }
    }
    
    return false
})

const anserText=ref('')
watch(anserText, value=>{
    if (value==props.value.toString())
        ev('finish', value, selectItem.value)
})
watch(()=>props.selectStatus, val=>{
    anserText.value=''
})

</script>

<template>
    <div class="tbItem"
            :class="[ isFinished ? 'bg-green-300' : (isSelected ? 'bg-blue-300' : 'bg-white')]">
        <p v-show="isShowText">{{ value }}</p>

        <input class="answer-input w-7 text-center" type="text" v-if="canWrite" v-model="anserText">
    </div>
</template>
