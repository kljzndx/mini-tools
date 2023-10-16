<script setup>
import { computed } from "vue";
import { GameSettings, GameModes, RandomModes } from "../../models/settings/gameSettings";

const props=defineProps({
    gameSettings:GameSettings,
    setSetting:Function,
})

/**
 * @callback SetSettingCallback
 * @param {GameSettings} the
 */

/**
 * 
 * @param {SetSettingCallback} setSettingCallback 
 */

function sendSetting(setSettingCallback){
    props.setSetting(setSettingCallback)
}

</script>
<template>
    <div>
        <div class="flex w-max gap-x-4
                p-5 bg-yellow-300 rounded-3xl border-gray-300 border-4">
        <div class="settingsGroup">
            <div>
                <label>
                    游戏模式：
                    <select :value="gameSettings.gameMode.id" @change="ev=>sendSetting(s=>s.gameMode=GameModes[ev.target.value-1])">
                        <option v-for="(item, id) in GameModes" :key="id" :value="item.id">{{ item.title }}</option>
                    </select>
                </label>
            </div>
            <div>
                <label>
                    阵列数：
                    <input type="number" :value="gameSettings.colsCount" @change="ev=>sendSetting(s=>s.colsCount=ev.target.value)">
                </label>
            </div>
        </div>
        <div class="settingsGroup">
            <div>
                <label>
                    <input type="checkbox" :checked="gameSettings.isBlindMode" @change="ev=>sendSetting(s=>s.isBlindMode=ev.target.checked)">
                    纯净模式
                </label>
            </div>
            <div>
                <label>
                    <input type="checkbox" :checked="gameSettings.isShowCrosshair" @change="ev=>sendSetting(s=>s.isShowCrosshair=ev.target.checked)">
                    显示十字准星
                </label>
            </div>
            <div>
                <label>
                    <input type="checkbox" :checked="gameSettings.isShowCols" @change="ev=>sendSetting(s=>s.isShowCols=ev.target.checked)">
                    显示列标
                </label>
            </div>
        </div>
        <div class="settingsGroup">
            <div>
                <label>
                    选择模式：
                    <select :value="gameSettings.randomMode.id" @change="ev=>sendSetting(s=>s.randomMode=RandomModes[ev.target.value-1])">
                        <option v-for="(item, id) in RandomModes" :key="id" :value="item.id">{{ item.title }}</option>
                    </select>
                </label>
            </div>
            <div>
                <label>
                    选择次数：
                    <input type="number" :value="gameSettings.selectTimes" @change="ev=>sendSetting(s=>s.selectTimes=ev.target.value)">
                </label>
            </div>
            <div>
                <label>
                    问题数量：
                    <input type="number" :value="gameSettings.questionCount" @change="ev=>sendSetting(s=>s.questionCount=ev.target.value)">
                </label>
            </div>
        </div>
    </div>
    </div>
</template>

<style scoped>
    input[type=number] {
        @apply w-16 border-2 border-black
    }

    select{
        @apply border-2 border-black
    }
    .settingsGroup{
        @apply flex flex-col gap-y-3
    }
</style>