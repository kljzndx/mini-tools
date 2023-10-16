<script setup>
//import HelloWorld from './components/HelloWorld.vue'
import { ref } from "vue";
import { GameSettings, GameModes, RandomModes } from "./models/settings/gameSettings";
import GamePanel from "./components/panel/gamePanel.vue";
import SettingsPanel from "./components/panel/settingsPanel.vue";

const gameSettings=ref(new GameSettings(GameModes[2], RandomModes[1], 10, 1, 3, false, false, true))

/**
 * @callback SetSettingCallback
 * @param {GameSettings} the
 */

/**
 * 
 * @param {SetSettingCallback} setSettingCallback 
 */

function setSetting(setSettingCallback){
  setSettingCallback(gameSettings.value)
}

</script>

<template>
  <div>
    <GamePanel :game-settings="gameSettings"/>
    <SettingsPanel :game-settings="gameSettings" :set-setting="setSetting"/>
  </div>
</template>

<style scoped>
</style>
