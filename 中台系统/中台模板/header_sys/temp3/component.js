"use strict";
function header_sys_temp3() {
	var header_sys_temp3_html = `<div class="header-temp3-page min-width">
	<div class="header-tip child_bg"></div>
	<div class="header-nav clearFloat">
		<div class="header-nav-content">
			<div class="left clearFloat">
				<img class="logo" @click="goHome()" :src="fileUrl+header_sys_temp3_list.siteLogo" v-if="header_sys_temp3_list.siteLogo">
				<a class="col" :class="header_sys_temp3_active(item)?'active':''" v-for="(item,index) in (header_sys_temp3_list.navigationColumnList[0]||[])" :key="index" @click="header_sys_temp3_openUrl(item.navigationUrl, item.isOpenNewWindow)">
				<span class="child_color_text_hover">{{item.navigationName}}</span>
				</a>
			</div>
			<div class="right">
				<span class="us-name" v-if="header_sys_temp3_isLogin()" @click="goUser()">您好：{{userInfo.name||''}}</span>
				<a class="col" v-for="(item,index) in (header_sys_temp3_list.navigationColumnList[1]||[])" :key="index" @click="header_sys_temp3_openUrl(item.navigationUrl, item.isOpenNewWindow)">
				<span class="child_color_text_hover">{{item.navigationName}}</span>
				</a>
				<!-- <a href="javascript:;" v-if="header_sys_temp3_list.personalLibrary" @click="header_sys_temp3_openUrl(header_sys_temp3_list.personalLibrary.navigationUrl, header_sys_temp3_list.personalLibrary.isOpenNewWindow)" class="btn-block child_bg">{{header_sys_temp3_list.personalLibrary.navigationName}}</a> -->
				<a v-if="header_sys_temp3_isLogin()" href="javascript:;" class="out" @click="header_sys_temp3_Out()"><img class="help-img" :src="fileUrl+'/public/image/icon-outlogin.png'">退出</a>
				<a href="javascript:;" v-if="!header_sys_temp3_isLogin()" @click="header_sys_temp3_login()" class="btn-block child_bg">登录</a>
				<!-- <a href="javascript:;" v-if="header_sys_temp3_list.helpInfo" @click="header_sys_temp3_openUrl(header_sys_temp3_list.helpInfo.navigationUrl, header_sys_temp3_list.helpInfo.isOpenNewWindow)"><img class="help-img" :src="fileUrl+'/public/image/help-icon.png'">{{header_sys_temp3_list.helpInfo.navigationName}}</a> -->
			</div>
		</div>
	</div>
</div>`;


	var list = document.getElementsByClassName('header_sys_temp3');
	for (var i = 0; i < list.length; i++) {
		if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
			list[i].setAttribute('class', 'header_sys_temp3 jl_vip_zt_vray');
			new Vue({
				el: '#' + list[i].lastChild.id,
				template: header_sys_temp3_html,
				data() {
					return {
						fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
                        post_url_vip: window.apiDomainAndPort,
						userInfo: JSON.parse(window.localStorage.getItem('userInfo') || '{}'),
						urlInfo:JSON.parse(localStorage.getItem('urlInfo')),
						header_sys_temp3_list: {
							navigationColumnList: [],
							personalLibrary: {},//我的书斋
						},
					}
				},
				mounted() {
					this.header_sys_temp3_initData();
				},
				methods: {
					//回首页
					goHome(){
						let info = this.urlInfo.find(item => item.code == 'index');
						window.location.href = info.path + '/#/index?page=1';
					},
					//到个人中心
					goUser(){
						let info = this.urlInfo.find(item => item.code == 'usermanage');
						window.location.href = info.path + '/usermanage/#/web_library';
					},
					//选中样式
					header_sys_temp3_active(val) {
						var url_hash = (location.hash||'1').length-1;
						if (val.navigationUrl.indexOf((location.hash.slice(0,url_hash)||'*'))>-1) {
							return true;
						} else {
							if(val.navigationUrl == location.origin && location.hash=='#/index'){
								return true;
							}else{
								return false;
							}
						}
					},
					//是否已登录
					header_sys_temp3_isLogin() {
						var token = window.localStorage.getItem('token');
						if (token && token.length > 5) {
							return true;//已登录
						} else {
							return false;
						}
					},
					//跳转链接
					header_sys_temp3_openUrl(url, isOpenNewWindow) {
						if (url) {
							let allUrl = url.indexOf('http') != -1 ? url : 'https://' + url;
							if (isOpenNewWindow) {
								window.open(allUrl, '_blank');
							} else {
								window.location.href = allUrl;
							}
						}
					},
					header_sys_temp3_login() {
						localStorage.removeItem('token');
						localStorage.setItem('COM+', window.location.href);
						window.location.href = window.casBaseUrl+'/cas/login?service=' + encodeURIComponent(window.location.origin + window.location.pathname)+'&orgcode='+window.orgcode
					},
					header_sys_temp3_Out() {
						if (confirm('确定要退出吗') == true) {
							localStorage.clear();
							sessionStorage.clear();
							localStorage.setItem('COM+', window.location.href);
							location.href = window.casBaseUrl+'/cas/logout?service=' + encodeURIComponent(window.location);
						} else {
							return false;
						}
					},
					header_sys_temp3_initData() {
						var post_url = this.post_url_vip+'/scenemanage/api/header-footer/header-data?templatecode=/header_sys/temp3';
						axios({
							url: post_url,
							method: 'GET',
							headers: {
								'Content-Type': 'text/plain',
								'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
							},
						}).then(res => {
							if (res.data && res.data.statusCode == 200) {
								this.header_sys_temp3_list = res.data.data || {navigationColumnList: [],personalLibrary: {}};
							}
						}).catch(err => {
							console.log(err);
						});
					},
				}
			});
		}
	}
}
header_sys_temp3()


