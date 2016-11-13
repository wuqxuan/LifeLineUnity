#!/usr/bin/env python3
# 
# 说明：该脚本仅适用于python3，不兼容python2.
# 运行：
# ➜  cd JsonData 
# ➜  python3 ExtractJson.py

# Written by @wuqxuan.
# 2016-11.

import sys
import os
import json
import re
from pprint import pprint

storyDataList = []    # 全局变量，StroyData按行读取生成的list
colonIndexs = []  # 前两次'::'的索引

# 读取txt文本到一个列表，然后对列表内一定格式的元素做拆分处理（对含有<<if>>  <<elseif>>  <<endif>>和文本的元素进行拆分）         
def fileLineFeed(lang):
    global colonIndexs
    colonCount = 0    # '::'出现的次数
    txtfile = 'Data/StoryData_' + lang + '.txt'
    with open(txtfile,'r') as f:
        global storyDataList    # 全局变量
        storyDataList = f.read().splitlines();    # 按行读取文本到一个 list，文本的每一行是一个 list 元素
    # 记录前两次‘::’的索引
    for line in storyDataList:
        if colonCount < 2:
            if ('::' in line):
                colonIndexs.append(storyDataList.index(line))
                colonCount += 1
    # 拆分if else endif 的行
    for line in storyDataList[:]:
        if len(line) and ('>> | <<' not in line):
            # <<if>>XX<<endif>>或<<elseif>>XX<<endif>>格式的行,拆分成3行
            if (line.find('>>', 1, len(line) - 1) != -1) and (line.find('<<', 1, len(line) - 1) != -1):
                actionTwoside = re.findall(r'([^\<]+)\>{2}([^\#]+)\<{2}([^\>]+)', line)
                i = storyDataList.index(line)
                # print(line) 
                storyDataList[i] = '<<' + actionTwoside[0][0] + '>>'
                storyDataList.insert(i+1, actionTwoside[0][1])
                storyDataList.insert(i+2,'<<' + actionTwoside[0][2] + '>>')
            # <<set>>XX或<<if>>XX或<<elseif>>XX格式，拆分成2行
            elif (line.find('>>', 1, len(line) - 1) != -1):
                actionLeft = re.findall(r'([^\<]+)\>{2}([^\#]+)', line)
                # print(actionLeft)
                i = storyDataList.index(line)
                storyDataList[i] = '<<' + actionLeft[0][0] + '>>'
                storyDataList.insert(i+1,actionLeft[0][1])
            # XX<<endif>>格式，拆分成2行
            elif (line.find('<<', 1, len(line) - 1) != -1):
                actionRight = re.findall(r'([^\#]+)\<{2}([^\>]+)', line)
                i = storyDataList.index(line)
                storyDataList[i] = actionRight[0][0]
                storyDataList.insert(i+1,'<<' + actionRight[0][1] + '>>')
    # 拆分XX[[delay
    for line in storyDataList[:]:
        if (line.find('[[', 1, len(line) - 1) != -1) and '<<' not in line:
            actionDelay = re.findall(r'([^\#]+)\[{2}([^\]]+)', line)
            i = storyDataList.index(line)
            storyDataList[i] = actionDelay[0][0]
            storyDataList.insert(i+1,'[[' + actionDelay[0][1] + ']]')
    # pprint(storyDataList)    
        
# 生成waypoints.json
def toScenes(lang):
    sceneKey = []
    sceneValue = []
    scenesAll = {}
    lineIndex = 0
    # 替换 action (‘]] | [[’ 所在的行)为 <<action lifelineXX>>
    for line in storyDataList[:]:
        if '::' in line:
            key = re.findall(r'\:{2}\s+([^#]+)', line)
            sceneKey.append(key[0])
        if ('>> | <<' in line):
            actionNum = 'lifeline' + str(lineIndex)
            actionIndex = storyDataList.index(line)
            storyDataList[actionIndex] = '<<category ' + actionNum + '>>'
            lineIndex = lineIndex + 1
            
    sceneStart = colonIndexs[0]
    while sceneStart < len(storyDataList):
        scenes = []
        for line in storyDataList[sceneStart + 1:]:
            
            if '::' not in line:
                if len(line):    # 非空行
                    scenes.append(line)
                # 最后一行
                # if storyDataList.index(line) == len(storyDataList) - 1:
                #     sceneStart = len(storyDataList) + 10000
                #     print('gameover')
                #     break
                
            # else:
            #     sceneStart = storyDataList.index(line)
            #     break
               
            else:
                if 'ENDSTORY' in line:    # while 循环结束
                    sceneStart = len(storyDataList) * 2
                    # print('end')
                    break
                else:
                    sceneStart = storyDataList.index(line)    # 跳到下一个‘::’的索引位置
                    # print(storyDataList[sceneStart])
                    break    # 跳出for循环
        sceneValue.append(scenes)
    scenesAll = dict(zip(sceneKey, sceneValue))
    scenes_file = 'Data/scenes_' + lang + '.json'
    with open(scenes_file,'w') as f:
	    json.dump(scenesAll,f, ensure_ascii = False)
    # print(scenesAll)
    
# 生成categories.json
def toChoices(lang):
    actionKey = ['choice', 'identifier']
    choiceKey = ['actions', 'identifier']
    choices = []
    lineIndex = 0
    for line in storyDataList[:]:
        acitonDict = []
        choiceValue = []
        if '<<choice' in line:
            actionSceneNum = 'lifeline' + str(lineIndex)
            actionAll = re.findall(r'([^\[\]\s\<\>]+)\|([^\[\]\s\<\>]+)', line)    # 匹配'|'两侧除‘［’和‘］’以及空格以外的元素，提取 action
            for action in actionAll:
                actionTemp = list(action)
                acitonDict.append(dict(zip(actionKey, actionTemp))) 
            choiceValue.append(acitonDict)
            choiceValue.append(actionSceneNum)
            choices.append(dict(zip(choiceKey, choiceValue)))
            lineIndex = lineIndex + 1 
    # pprint(choices)
    choices_file = 'Data/choices_' + lang + '.json'
    with open(choices_file,'w') as f:
	    json.dump(choices, f, ensure_ascii = False) # ensure_ascii=False 处理中文
           
if __name__ == '__main__':
    fileLineFeed('cn')
    # toStatus()
    toChoices('cn')
    toScenes('cn')