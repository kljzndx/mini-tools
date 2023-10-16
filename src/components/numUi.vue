<script setup>
    const props = defineProps({
        value:Number, 
        allMoney:Number, 
        isSelected:Boolean,
    })
    const emit=defineEmits(['onChange'])
</script>
<template>
    <div class="w-20 h-12 border-2 border-black flex flex-col box-border items-center">
        <div>
            <label>
                <input type="checkbox" :checked="isSelected" @change="$emit('onChange', {isChecked:$event.target.checked, value:value})">
                {{ value.toString().padStart(2, '0') }}
            </label>
        </div>
        <p>总:{{ allMoney }}</p>
    </div>
</template>
