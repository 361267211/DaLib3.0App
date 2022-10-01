function other_left_menu_list() {
  var other_left_menu_list_html = `<div class="temp-left_menu_list-warp">
<div class="search-warp">
  <div class="unified_retrieval_sys_temp1" data-set="[{'topCount':5,'sortType':'CreatedTime-DESC','id':'15164479-81e6-40d1-9cc5-17fefa3905ae'}]">
    <div id="unified_retrieval_sys_temp1"><div class="temp-loading-medium"></div><!--加载中--></div>
  </div>
</div>

<div class="hot-apps">
    <div class="main-title-temp"><span>推荐应用</span><span class="e-title"></span></div>
    <div class="apps-conent c-l">
        <div class="box" v-for="(it,i) in other_left_menu_list_list3" @click="other_left_menu_list_openurl(it.frontUrl)" v-if="i<6">
            <div class="apps-c-logo"><img :src="fileUrl+it.appIcon"></div>
            <span :title="it.appName">{{it.appName}}</span>
        </div>
        <div class="temp-loading-medium" v-if="request_of1"></div><!--加载中-->
        <div class="jl_vip_zt_empty_data" v-if="!request_of1 && other_left_menu_list_list3 && other_left_menu_list_list3.length==0">暂无内容</div><!--暂无内容-->
    </div>
</div>
<div class="plase-login" v-if="!isLogin()" :style="{'background-image': 'url('+fileUrl+'/public/image/home01.png),url('+fileUrl+'/public/image/home02.png)'}">
  <div class="welcome-txt">Hi！您好</div>
  <div class="hint-txt">请登录您的专属图书馆</div>
  <a class="btn" href="javascript:;" @click="loginClick()">点击登录</a>
</div>
<div class="my-study" v-if="isLogin()">
    <div class="main-title-temp"><span>我的图书馆</span><span class="e-title"></span></div>
    <div class="num-count-warp c-l">
        <div class="n-c-box" v-for="(it,i) in other_left_menu_list_list1" :class="(i+1)==other_left_menu_list_list1.length?'c-b-clear':''" @click="other_left_menu_list_openurl(it.linkUrl)">
            <span class="num">{{it.count||0}}</span>
            <span>{{it.name}}</span>
        </div>
        <div class="temp-loading-medium" v-if="request_of"></div><!--加载中-->
        <div class="jl_vip_zt_empty_data" v-if="!request_of && other_left_menu_list_list1 && other_left_menu_list_list1.length==0">暂无内容</div><!--暂无内容-->
    </div>
    <div class="main-title-temp"><span>最新成果</span></div>
    <div class="like-warp">
        <div class="like-row" v-for="(it,i) in other_left_menu_list_list2" v-if="i<5" @click="other_left_menu_list_openurl(it.detailLink)">
            <div class="like-title"><span class="type" :class="typeClass(it.achievementType)">{{articleTypes(it.achievementType)}}</span>{{it.title}}</div>
            <div>{{it.creator}},{{it.creator_institution}} {{it.date}}</div>
        </div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && other_left_menu_list_list2 && other_left_menu_list_list2.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无内容-->
    </div>
</div>
</div>`;

  var doc_element = document;

  var list = document.getElementsByClassName('other_left_menu_list');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'other_left_menu_list jl_vip_zt_vray jl_vip_zt_warp');
      var other_left_menu_list_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        other_left_menu_list_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));//第一个是检索，第二个是推荐应用
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: other_left_menu_list_html,
        data() {
          return {
            request_of:true,//请求中
            request_of1:true,//请求中
            other_left_menu_list_list1: [],
            other_left_menu_list_list2: [],
            other_left_menu_list_list3: [],
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          this.onlineData();
          this.other_left_menu_list_initData1();
          //下面挨着两行需要删除-目前先写死
          var list = { 'id': '1067c97d-2d96-4935-b1c3-e10f9940e6a8', 'name': null, 'visitUrl': null, 'createTime': '0001-01-01T00:00:00', 'topCount': 6, 'sortType': 'CreatedTime-DESC' };
          this.other_left_menu_list_initData2(list.topCount, list.id, list.sortType);

          if (other_left_menu_list_set_list.length > 0) {
            other_left_menu_list_set_list.forEach((it, i) => {
              var set_content = it;
              var topCount = '';
              var columnid = '';
              var OrderRule = '';
              if (set_content) {
                topCount = set_content['topCount'] || 16;
                columnid = set_content['id'];
                OrderRule = set_content['sortType'];
              }
              if (i == 0) {//检索

              } else if (i == 1) {//推荐应用
                console.log('推荐应用');
                var list = { 'id': '1067c97d-2d96-4935-b1c3-e10f9940e6a8', 'name': null, 'visitUrl': null, 'createTime': '0001-01-01T00:00:00', 'topCount': 6, 'sortType': 'CreatedTime-DESC' };
                this.other_left_menu_list_initData2(list.topCount, list.id, list.sortType);
              }
            });
          }
        },
        methods: {
          //是否已登录
          isLogin() {
            var token = window.localStorage.getItem('token');
            if (token && token.length > 5) {
              return true;//已登录
            } else {
              return false;
            }
          },
          //登录
          loginClick() {
            localStorage.removeItem('token');
            localStorage.setItem('COM+', window.location.href);
            window.location.href = window.casBaseUrl+'/cas/login?service=' + encodeURIComponent(window.location.origin + window.location.pathname)+'&orgcode='+window.orgcode
          },
          //加载检索文件
          onlineData() {
            console.log('加载检索文件');
            var unified_retrieval_url = 'http://192.168.21.71:9000/unified_retrieval_sys/temp1';
            this.addStyle(unified_retrieval_url + '/component.css');
            this.addScript(unified_retrieval_url + '/component.js');
          },
          addStyle(url) {
            var link = doc_element.createElement("link");
            link.setAttribute("rel", "stylesheet");
            link.setAttribute("type", "text/css");
            link.setAttribute("href", url + '?version=' + new Date().getTime());
            doc_element.getElementsByTagName("body")[0].appendChild(link);
          },
          addScript(url) {
            var js_element = doc_element.createElement("script");
            js_element.setAttribute("type", "text/javascript");
            js_element.setAttribute("src", url + '?version=' + new Date().getTime());
            doc_element.getElementsByTagName("body")[0].appendChild(js_element);
          },
          other_left_menu_list_openurl(url) {
            window.open(url);
          },
          //推荐应用
          other_left_menu_list_initData2(topCount, columnid, OrderRule) {
            axios({
              url: this.post_url_vip + '/appcenter/api/userapplication/getrecommendapps',
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('BasicToken'),
              },
            }).then(res => {
              this.request_of1 = false;
              if (res.data && res.data.statusCode == 200) {
                // this.other_left_menu_list_list3 = res.data.data.apps||[];
                this.other_left_menu_list_list3 = res.data.data || [];
              }
            }).catch(err => {
              this.request_of1 = false;
              console.log(err);
            });
          },
          //我的图书馆-最新成果
          other_left_menu_list_initData1() {
            axios({
              url: this.post_url_vip + '/usermanage/api/scene/my-study-info',
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('BasicToken'),
              },
            }).then(res => {
              this.request_of = false;
              if (res.data && res.data.statusCode == 200) {
                this.other_left_menu_list_list1 = res.data.data.items || [];
                this.other_left_menu_list_list2 = res.data.data.recommends || [];
              }
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
          typeClass(val){
            switch (val) {
              case 1: return 'bg1';
              case 2: return 'bg2';
              case 3: return 'bg3';
              case 4: return 'bg4';
            }
          },
          articleTypes(val) {
            switch (val) {
              case 1: return '我的成果';
              case 2: return '待领成果';
              case 3: return '本院成果';
              case 4: return '本校成果';
              // case 1: return '图书';
              // case 2: return '期刊';
              // case 3: return '期刊文献';
              // case 4: return '学位论文';
              // case 5: return '标准';
              // case 6: return '会议';
              // case 7: return '专利';
              // case 8: return '法律法规';
              // case 9: return '成果';
              // case 10: return '多媒体';
              // case 11: return '报纸';
              // case 12: return '科技报告';
              // case 13: return '产品样本';
              // case 14: return '资讯';
            }
          }
        },
      });
    }
  }
}
other_left_menu_list()
