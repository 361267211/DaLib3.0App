function literature_recommend_sys_temp7() {
  var literature_recommend_sys_temp7_html = `<div class="temp-recommendthejournal-warp-pad">
  <div class="temp-recommendthejournal-warp">
      <div class="top-main-title-temp"><span>{{literature_recommend_sys_temp7_list.name||'无'}}</span><span class="e-title"></span><span class="r-btn" @click="literature_recommend_sys_temp7_openurl(literature_recommend_sys_temp7_list.moreUrl)">查看更多 ></span></div>
      <div class="temp-loading" v-if="request_of"></div><!--加载中-->
      <div class="web-empty-data" v-if="!request_of && literature_recommend_sys_temp7_list && (literature_recommend_sys_temp7_list.titleInfos||[]).length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" ></div><!--暂无数据-->
      <div class="recommendthejournal-list-warp clearFloat" v-if="literature_recommend_sys_temp7_list.titleInfos">
          <div class="recommendthejournal-box" v-for="it in literature_recommend_sys_temp7_list.titleInfos">
              <div class="recommendthejournal-box-bg">
                  <img :src="it.cover" class="book-cover" :onerror="default_img"  @click="literature_recommend_sys_temp7_openurl(it.detail_url)"/>
                  <div class="book-info">
                      <p class="book-name" @click="literature_recommend_sys_temp7_openurl(it.detail_url)">{{it.title}}</p>
                      <p class="book-author">{{it.creator||'无'}}</p>
                      <p class="book-i-gary">eISSN：{{it.identifier_eissn||'-'}}</p>
                      <p class="book-i-gary">pISSN：{{it.identifier_pissn||'-'}}</p>
                      <p class="book-i-gary">出版社：{{it.publisher||'未知'}}</p>
                      <p class="type-warp" v-if="it.description_core">
                          <span class="type-btn" v-for="i in it.description_core.split(';')">{{i}}</span>
                      </p>
                  </div>
              </div>
          </div>
      </div>
  </div>
</div><!--推荐期刊-->`;

  var list = document.getElementsByClassName('literature_recommend_sys_temp7');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'literature_recommend_sys_temp7 jl_vip_zt_vray jl_vip_zt_warp');
      var literature_recommend_sys_temp7_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        literature_recommend_sys_temp7_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: literature_recommend_sys_temp7_html,
        data() {
          return {
            request_of:true,//请求中
            literature_recommend_sys_temp7_list: {},//当前列表数据
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            default_img: 'this.src="'+window.localStorage.getItem('fileUrl')+'/public/image/img-error-174x241.jpg"',
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          if (literature_recommend_sys_temp7_set_list) {
            if (literature_recommend_sys_temp7_set_list.length > 0) {
              var list = [];
              literature_recommend_sys_temp7_set_list.forEach((it, i) => {
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
              this.literature_recommend_sys_temp7_initData(list);
            }
          }
        },
        methods: {
          literature_recommend_sys_temp7_openurl(url) {
            window.open(url)
          },
          literature_recommend_sys_temp7_initData(list) {
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
                  this.literature_recommend_sys_temp7_list = res.data.data[0] || {};
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
literature_recommend_sys_temp7()