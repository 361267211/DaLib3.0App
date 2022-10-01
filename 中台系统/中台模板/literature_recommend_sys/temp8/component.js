function literature_recommend_sys_temp8() {
  var literature_recommend_sys_temp8_html = `<div class="bookschannel-left-menu-pad">
  <div class="bookschannel-left-menu">
    <div class="hxksl-warp">
        <div class="img-title-main hxksl-bg"></div>
        <div class="hxksl-list" v-if="literature_recommend_sys_temp8_list[0] && literature_recommend_sys_temp8_list[0].cores">
            <a href="javascript:;" v-for="i in literature_recommend_sys_temp8_list[0].cores" @click="literature_recommend_sys_temp8_openurl(literature_recommend_sys_temp8_list[0].moreUrl,i.value)">{{i.key}}</a>
        </div>
    </div>
    <div class="xkdh-warp">
        <div class="img-title-main xkdh-bg"></div>
        <div class="xkdh-list" v-if="literature_recommend_sys_temp8_list[1] && literature_recommend_sys_temp8_list[1].domainInfos">
            <div class="xkdh-l-row" v-for="i in literature_recommend_sys_temp8_list[1].domainInfos">
                <span>{{i.domainName}}</span>
                <span class="r-btn"></span>
                <div class="xkdh-menu" v-if="i.children">
                  <a :href="literature_recommend_sys_temp8_list[1].moreUrl+'&code='+ite.domainIdCode" v-for="ite in i.children">{{ite.domainName||'-'}}</a>
                </div>
            </div>
        </div>
    </div>
  </div>
</div><!--核心刊收录、学科导航-->`;

  var list = document.getElementsByClassName('literature_recommend_sys_temp8');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'literature_recommend_sys_temp8 jl_vip_zt_vray jl_vip_zt_warp');
      var literature_recommend_sys_temp8_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        literature_recommend_sys_temp8_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: literature_recommend_sys_temp8_html,
        data() {
          return {
            literature_recommend_sys_temp8_list: [],//当前列表数据
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            hxksl_url: '',//核心刊收录
            xkdh_url: '',//学科导航
          }
        },
        mounted() {
          if (literature_recommend_sys_temp8_set_list) {
            if (literature_recommend_sys_temp8_set_list.length > 0) {
              var list = [];
              literature_recommend_sys_temp8_set_list.forEach((it, i) => {
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
              this.literature_recommend_sys_temp8_initData(list);
            }
          }
        },
        methods: {
          literature_recommend_sys_temp8_openurl(url, val) {
            window.open(url + '&code=' + val);
          },
          literature_recommend_sys_temp8_initData(list) {
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
              if (res.data && res.data.statusCode == 200) {
                this.literature_recommend_sys_temp8_list = res.data.data || [];
                if (res.data.data.length > 0) {
                  // this.literature_recommend_sys_temp8_menu_list = res.data.data[0].titleInfos||[];
                }
              }
            }).catch(err => {
              console.log(err);
            });
          },
        },
      });
    }
  }
}
literature_recommend_sys_temp8()