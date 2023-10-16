<script setup>
import { computed, ref } from 'vue'
import {NumList} from './models/numList'
import NumUi from './components/numUi.vue';
import ClipboardJS from 'clipboard';

const isCopied=ref(false)

const cb=new ClipboardJS('.copybtn')
cb.on('success', e=>isCopied.value=true)
cb.on('error', e=>alert('复制失败'))

const numberArray=ref([])
for (let i = 0; i < 49; i++) {
  numberArray.value.push({value:i+1, allMoney:0, isSelected:true})
}

const selected=computed(()=>{
  return numberArray.value.filter(v=>v.isSelected).map(v=>v.value)
})

const spSymbol=ref('-:|*')
const itemCount=ref(28)
const allMoney=ref(30000)
const eachMoney_min=ref(50)
const eachMoney_max=ref(1000)
const numCount_min=ref(8)
const numCount_max=ref(12)

const resultList=ref([])

function cleanAll() {
  resultList.value=[]
  numberArray.value.forEach(v=>v.allMoney=0)
  isCopied.value=false
}

function randomBuildData() {
  cleanAll()

  const chaZhi=numCount_max.value-numCount_min.value
  const resList=[]
  let money=0
  let seltList=selected.value.map(v=>v)

  while (resList.length<itemCount.value) {
    const addValue=Math.floor(Math.random()*(chaZhi+1))
    const numCount=numCount_min.value+addValue

    const nums=[]

    while (nums.length<numCount&&nums.length<selected.value.length) {
      if (seltList.length==0)
        seltList=selected.value.map(v=>v)

      const id=Math.floor(Math.random()*seltList.length)
      const val = seltList[id]
      if (nums.includes(val))
        continue

      nums.push(val)
      seltList.splice(id, 1)
    }
    
    resList.push(new NumList(nums, spSymbol.value, eachMoney_min.value))
    money+=(eachMoney_min.value*nums.length)
  }

  let m_fall=0
  while (money<allMoney.value&&m_fall<10) {
    m_fall++

    for (const ns of resList) {
      if (money>allMoney.value)
        break
      if (ns.eachMoney>eachMoney_max)
        continue

      ns.eachMoney+=10
      money+=(10*ns.nums.length)
      m_fall=0
    }
  }

  resList.forEach(v=>{
    resultList.value.push(v)
    v.nums.forEach(r=>numberArray.value[r-1].allMoney+=v.eachMoney)
  })
}

const realAllMoney=computed(()=>{
  let res=0
  numberArray.value.forEach(v=>res+=v.allMoney)
  return res
})
const resultStr=computed(()=>{
  let str=''
  resultList.value.forEach(v=>str+=v.toString()+'\r\n')
  return `实际总花费: ${realAllMoney.value} \r\n\r\n`+str
})
</script>

<template>
  <div class=" w-min m-4">
    <div class="w-[51rem] flex flex-wrap gap-3">
      <NumUi v-for="num of numberArray" :value="num.value" :all-money="num.allMoney" :is-selected="num.isSelected" @on-change="({isChecked, value})=>num.isSelected=isChecked"/>
      <button class="btn" @click="()=>numberArray.forEach(v=>v.isSelected=!v.isSelected)">反选</button>
      <button class="btn" @click="()=>numberArray.forEach(v=>v.isSelected=false)">清除</button>
    </div>

    <div class="w-max my-4 mx-auto">
      <div class=" w-[41rem] flex flex-wrap gap-2">
        <div>
          <label>
            条目数量：
            <input class="w-20" type="text" v-model.number="itemCount">
          </label>
        </div>
        <div>
          <label>
            我的预算：
            <input class="w-20" type="text" v-model.number="allMoney">
          </label>
        </div>
        <div>
          <label>
            间隔符随机：
            <input class="w-20" type="text" v-model.number="spSymbol">
          </label>
        </div>
        <div>
          <label>
            单号金额区间：
            <input class="w-20" type="text" placeholder="最小值" v-model.number="eachMoney_min">
          </label>
          <label>
            ~
            <input class="w-20" type="text" placeholder="最大值" v-model.number="eachMoney_max">
          </label>
        </div>
        <div>
          <label>
            单条数量区间：
            <input class="w-20" type="text" placeholder="最小值" v-model.number="numCount_min">
          </label>
          <label>
            ~
            <input class="w-20" type="text" placeholder="最大值" v-model.number="numCount_max">
          </label>
        </div>
      </div>
      <div class="mx-auto flex gap-x-2">
        <button class="btn" @click="randomBuildData">生成列表</button>
        <button class="copybtn btn" :data-clipboard-text="resultStr">{{ isCopied?'复制成功':'复制列表' }}</button>
      </div>
    </div>
    <div class="my-4">
        <label>
          条目列表:
          <textarea id="resultTextArea" class="w-[51rem] block bg-gray-200" rows="8" disabled :value="resultStr"></textarea>
        </label>
    </div>
  </div>

</template>

<style scoped>
  input[type=text],textarea{
    @apply border-2 border-black px-1
  }
  .btn{
    @apply p-2 bg-blue-400 hover:bg-blue-300 active:bg-blue-500
  }
</style>
