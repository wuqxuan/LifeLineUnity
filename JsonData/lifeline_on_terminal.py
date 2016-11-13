#!/usr/bin/env python
# coding: utf-8

# 参考
# http://www.jianshu.com/p/5a480d2d5dc6
# https://github.com/Urinx/Lifeline_SilentNight

# 说明：该脚本仅适用python2.
# 运行：
# ➜  cd JsonData 
# ➜  python lifeline_on_terminal.py

import sys
import os
import json
import random
import re
from time import sleep

def echo(str):
	sys.stdout.write(str)
	# 刷新缓冲区
	sys.stdout.flush()

class Story(object):
	def __init__(self):
		self.status = {}
		self.scenes = {}
		self.choices = {}
		
		self.loadStatusData()
		self.loadStoryData('cn')
        # self.fastMode = False

	def loadStoryData(self, lang):
		scene_file = 'Data/scenes_' + lang + '.json'
		choice_file = 'Data/choices_' + lang + '.json'

		with open(scene_file,'r') as f:
			self.scenes = json.load(f)

		with open(choice_file,'r') as f:
			self.choices = json.load(f)

	def loadStatusData(self):
		status_file = 'Data/status.json'
		if os.path.exists(status_file):
			with open(status_file,'r') as f:
				self.status = json.load(f)
				
		else:
			self.status = {
				'Settings': {
					'atScene': 'Start'
				}
			}
			
	def saveStatusData(self, scene):
		self.status['Settings']['atScene'] = scene
		status_file = 'Data/status.json'
		with open(status_file,'w') as f:
			# 转换为 json
			json.dump(self.status, f)

	def handleJump(self, line):
		line = line[2:-2]
		if line[:5] == 'delay':
			[delay, line] = line.split('|')
			# print(line)
			delay_time = delay.replace('s',')').replace('m','*60)').replace('h','*3600)').replace('delay ','sleep(')
			# if not self.fastMode: eval(delay_time) # 快速模式
	   
		self.status['Settings']['atScene'] = line

	def handleSet(self, line):
		# line = line[7:-2].replace(' ','').split('=')
		# self.status[line[0]] = line[1]    # 写入字典，若不存在，则增加键值对
		# print(len(self.status))
		params = re.findall(r'([^$]+)\s\=\s([^\>]+)', line)  
		if '- 1' in params[0][1]:
			self.status[params[0][0]] = str(int(self.status[params[0][0]]) - 1)    # pills or glowrod 
		else:
			self.status[params[0][0]] = params[0][1]

	def handleTalk(self, line):
		if "$pills" in line or "$glowrods" in line or "$power" in line:
			newLine = line.replace("$pills", self.status["pills"]).replace("$glowrods", self.status["glowrods"]).replace("$power", self.status["power"])
			print newLine,'\n'
		else:
			print line,'\n'
		# print line,'\n'
		sleep(.1)
		# if not self.fastMode: sleep(2.5)    # 快速模式 

	def handleChoice(self, line, scene):
		# line[19:-2] = lifeline编码
		choice = self.choices[int(line[19:-2])]["actions"]
		# 选项:黄色
		echo('\033[33m')
		print '0.',choice[0]['choice']
		print '1.',choice[1]['choice'],'\033[0m'
		while 1:
			try:
				i = raw_input('>: ')
			except:
				print '\n\n[*] Quit'
				print '[*] Save game data\n'
				self.saveStatusData(scene)
				exit()
			if not i:
				# i = str(random.randint(0,1)) # this is for test
				continue
			# if not i:
			# 	i = str(random.randint(0,1)) # this is for test, press 'ENTERT' key will give random choice.
			# 	# continue
			if i.isdigit() and int(i) in (0,1):
				i = int(i)
				# 我的回答:黄色
				print '\n\033[33m'+choice[i]['choice']+'\033[0m\n'
				self.status['Settings']['atScene'] = choice[i]['identifier']
			
				break
			else:
				print 'Please input a number (0-1)'
    # 循环执行
	def atScene(self, scene):
		# print(scene)
		self.status['Settings']['atScene'] = None    # gameover Non时候，atScene（）在运行
		if_else = False
		skip_line = False
		# scenes is waypoints 
		for line in self.scenes[scene]:
			# print(line)
			
		    # 处理 if else endif，两个 if 不能交换顺序
			if if_else:
				if line[:6] == '<<else':
					skip_line = not skip_line
					
					continue    # 下一行
				elif line == '<<endif>>':
					if_else = False
					continue

				if skip_line: continue
				
			if line[:4] == '<<if':
				if_else = True 
				# self.status['key'] == 'false'
				if_line = line[5:-2].replace('false','\'false\'').replace('true','\'true\'')
				if_line = if_line.replace(' is','\'] ==').replace(' gte','\'] >=').replace('$','self.status[\'')
	
				skip_line = False if eval(if_line) else True    # 执行字符串，if eval(if_line) 为 true，则处理下一行，否则执行 else
				
			# 设置参数
			elif line[:5] == '<<set': self.handleSet(line)
			# 跳转scene
			elif line[:2] == '[[': self.handleJump(line)
			# 进入choice
			elif line[:10] == '<<category': self.handleChoice(line, scene)
			# 弹出对话
			else: self.handleTalk(line)   # 最后一句

	def start(self):
		while self.status['Settings']['atScene'] is not None:
			# print('scene not none')
			self.atScene(self.status['Settings']['atScene'])
		self.saveStatusData('Start')    # gameover


if __name__ == '__main__':
	story = Story()
	story.start()
