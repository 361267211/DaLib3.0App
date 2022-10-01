function literature_recommend_sys_temp4() {
  var literature_recommend_sys_temp4_html = `<div class="news-book-express-pad">
    <div class="news-book-express">
        <div class="top-main-title-temp"><span>{{literature_recommend_sys_temp4_list.name|'无'}}</span><span class="e-title"></span><span class="r-btn" @click="literature_recommend_sys_temp4_openurl(literature_recommend_sys_temp4_list.moreUrl)">查看更多 ></span></div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && literature_recommend_sys_temp4_list && literature_recommend_sys_temp4_list.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
        <div class="book-list clearFloat">
            <div class="box-25" v-for="(it,i) in (literature_recommend_sys_temp4_list.titleInfos||[])" @click="literature_recommend_sys_temp4_openurl(it.detail_url)">
                <img :src="it.cover" :onerror="default_img">
                <p class="title">{{it.title}}</p>
                <p class="s-title">{{it.creator}}</p>
            </div>
        </div>
    </div>
</div><!--新书速递-->`;

  var list = document.getElementsByClassName('literature_recommend_sys_temp4');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'literature_recommend_sys_temp4 jl_vip_zt_vray jl_vip_zt_warp');
      var literature_recommend_sys_temp4_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        literature_recommend_sys_temp4_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: literature_recommend_sys_temp4_html,
        data() {
          return {
            request_of:true,//请求中
            literature_recommend_sys_temp4_list: {},//当前列表数据
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            default_img: 'this.src="'+window.localStorage.getItem('fileUrl')+'/public/image/img-error-174x241.jpg"',
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          if (literature_recommend_sys_temp4_set_list) {
            if (literature_recommend_sys_temp4_set_list.length > 0) {
              var list = [];
              literature_recommend_sys_temp4_set_list.forEach((it, i) => {
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
              this.literature_recommend_sys_temp4_initData(list);
            }
          }
        },
        methods: {
          literature_recommend_sys_temp4_openurl(url) {
            window.open(url)
          },
          literature_recommend_sys_temp4_initData(list) {
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
                if (res.data.data.length > 0) {
                  this.literature_recommend_sys_temp4_list = res.data.data[0] || {};
                }

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
literature_recommend_sys_temp4()