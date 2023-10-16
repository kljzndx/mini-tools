<script setup>
import { computed, ref, watch } from "vue";
import { RandomItemsWarper } from "../../models/settings/randomModes";

const props=defineProps({
    id:Number,
    text:String,
    orientation:{
        type:String,
        validator(val){
            return ['x', 'y'].includes(val)
        }
    },
    selectStatus:RandomItemsWarper,
    isBlindMode:Boolean,
    isShowCols:Boolean,
})

const isSelected=computed(()=>{
    if (props.selectStatus!=undefined&&props.isShowCols){
        for (const item of props.selectStatus.items) {
            if (item.point[props.orientation]==props.id){
                return true
            }
        }
    }
    return false
})

</script>

<template>
    <div class="tbItem" 
        :class="[isSelected?'bg-blue-500':'bg-gray-200']">
        <p v-show="isShowCols && (isSelected || !isBlindMode)">{{ text }}</p>
    </div>
</template>
