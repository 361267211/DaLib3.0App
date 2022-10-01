function literature_recommend_sys_temp6() {
  var literature_recommend_sys_temp6_html = `<div class="tab-navigation-pad">
  <div class="tab-navigation">
    <div class="top-main-title-temp"><span>标签导航</span><span class="e-title"></span></div>
    <div class="temp-loading" v-if="request_of"></div><!--加载中-->
    <div class="web-empty-data" v-if="!request_of && literature_recommend_sys_temp6_list && literature_recommend_sys_temp6_list.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" ></div><!--暂无数据-->
    <div class="tab-row-warp" v-for="(it,i) in literature_recommend_sys_temp6_list">
        <div class="tab-r-title">{{it.name}}</div>
        <div class="tab-r-list">
            <a href="javascript:;" class="tab-box" v-for="ite in it.subjectNames" @click="literature_recommend_sys_temp6_openurl(ite.detailUrl)">{{ite.word}}</a>
            <!--<a href="javascript:;" class="more">更多&nbsp;></a>--></a>
        </div>
    </div>
  </div>
</div><!--标签导航-->`;

  var list = document.getElementsByClassName('literature_recommend_sys_temp6');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'literature_recommend_sys_temp6 jl_vip_zt_vray jl_vip_zt_warp');
      var literature_recommend_sys_temp6_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        literature_recommend_sys_temp6_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: literature_recommend_sys_temp6_html,
        data() {
          return {
            request_of:true,//请求中
            literature_recommend_sys_temp6_list: [],
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          if (literature_recommend_sys_temp6_set_list) {
            if (literature_recommend_sys_temp6_set_list.length > 0) {
              var list = [];
              literature_recommend_sys_temp6_set_list.forEach((it, i) => {
                var topCount = '';
                var columnid = '';
                var OrderRule = '';
                if (it) {
                  topCount = it['topCount'] || 16;
                  columnid = it['id'];
                  OrderRule = it['sortType'];
                }
                list.push({ Count: topCount, ColumnId: columnid, OrderRule: OrderRule });
              });
              this.literature_recommend_sys_temp6_initData(list);
            }
          }
        },
        methods: {
          literature_recommend_sys_temp6_openurl(url) {
            window.open(url)
          },
          literature_recommend_sys_temp6_initData(list) {
            var post_url = this.post_url_vip + '/articlerecommend/api/sceneuse/getsceneusebyidbatch';
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
                this.literature_recommend_sys_temp6_list = res.data.data || [];
              }
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
        },
      });
    }
  }
}
literature_recommend_sys_temp6()