function database_nav_sys_temp1() {
  var database_nav_sys_temp1_html = `<div class="temp-database-warp">
<div class="main-title-temp"><span>常用数据库</span><span class="e-title"></span></div>
  <div class="database-content c-l">
      <div class="database_nav_sys_temp1_data-list c-l">
          <div class="data-box" v-for="i in database_nav_sys_temp1_list1" @click="database_nav_sys_temp1_openUrl(i)">
              <div class="d-box c-l">
                  <img class="d-logo" :src="fileUrl+i.cover">
                  <div class="data-txt">
                      <span>{{i.title}}</span>
                      <span>{{i.introduction}}</span>
                  </div>
              </div>
          </div>
          <div class="temp-loading" v-if="request_of"></div><!--加载中-->
          <div class="web-empty-data" v-if="!request_of && database_nav_sys_temp1_list1 && database_nav_sys_temp1_list1.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
          </div><!--暂无数据-->
      </div><!--常用数据库 end-->

      <div class="hot-list">
          <div class="hot-title"><i>荐</i>推荐数据库</div>
          <ul>
              <li v-for="i in database_nav_sys_temp1_list2" @click="database_nav_sys_temp1_openUrl(i)">{{i.title}}</li>
          </ul>
          <div class="temp-loading" v-if="request_of"></div><!--加载中-->
          <div class="web-empty-data" v-if="!request_of && database_nav_sys_temp1_list2 && database_nav_sys_temp1_list2.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
          </div><!--暂无数据-->
      </div><!--推荐数据库 end-->
  </div>
</div>`;


  var list = document.getElementsByClassName('database_nav_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'database_nav_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var database_nav_sys_temp1_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: database_nav_sys_temp1_html,
        data() {
          return {
            request_of:true,//请求中
            database_nav_sys_temp1_list1: [],
            database_nav_sys_temp1_list2: [],
            post_url_vip: window.apiDomainAndPort,
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
          }
        },
        mounted() {
          if (database_nav_sys_temp1_set_list) {
            var list = [];
            for (var i = 0; i < database_nav_sys_temp1_set_list.length; i++) {
              var topCount = database_nav_sys_temp1_set_list[i].topCount;
              var columnid = database_nav_sys_temp1_set_list[i].id;
              list.push({ count: topCount, columnid: columnid });
            }
            this.database_nav_sys_temp1_initData(list);
          }
        },
        methods: {
          database_nav_sys_temp1_initData(list) {
            var post_url = this.post_url_vip + '/databaseguide/api/database-terrace/database-in-portal';
            axios({
              url: post_url,
              method: 'post',
              data: list,
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              this.request_of = false;
              if (res.data && res.data.statusCode == 200) {
                if (res.data.data.length > 0) {
                  this.database_nav_sys_temp1_list1 = res.data.data[0].list || [];
                  if (res.data.data[1]) {
                    this.database_nav_sys_temp1_list2 = res.data.data[1].list || [];
                  }

                }
              }
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
          database_nav_sys_temp1_openUrl(val) {
            if(val.directUrls && val.directUrls[0]){
              let url = val.directUrls[0].url;
              url = url.indexOf('http') != -1 ? url : 'https://' + url;
              axios({
                url: this.post_url_vip + '/databaseguide/api/database-terrace/visit-databases?databaseid=' + val.id,
                method: 'GET',
                headers: {
                  'Content-Type': 'application/json',
                  'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                },
              }).then(res => {
  
              }).catch(err => {
                console.log(err);
              });
              window.open(url);
            }else{
              // window.open(val.portalUrl);
            }
          },
        },
      });
    }
  }
}
database_nav_sys_temp1()