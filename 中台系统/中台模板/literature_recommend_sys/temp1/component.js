function literature_recommend_sys_temp1() {
  var literature_recommend_sys_temp1_html = `<div class="periodical-nav1-warp">
<div class="main-title-temp"><span>期刊导航</span><span class="e-title"></span>
    <a :class="literature_recommend_sys_temp1_menu_num==i?'periodical-nav-active':''" href="javascript:;" v-for="(it,i) in literature_recommend_sys_temp1_list" @click="menuClick(i)">{{it.name}}</a>
</div>
<div class="special1-banner-warp" ref="literature_recommend_sys_temp1_ref">
    <div class="special1-b-pre" @click="literature_recommend_sys_temp1_preClick()"></div>
    <div class="special1-b-next" @click="literature_recommend_sys_temp1_nextClick()"></div>
    <div class="special1-banner-warp-pad">
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && literature_recommend_sys_temp1_menu_list && literature_recommend_sys_temp1_menu_list.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
        <div class="s-b-warp" v-if="literature_recommend_sys_temp1_menu_list && literature_recommend_sys_temp1_menu_list.length>0" :style="{'margin-left':-literature_recommend_sys_temp1_margin_left+'px'}">
            <div class="box" v-for="(it,i) in literature_recommend_sys_temp1_menu_list" @click="literature_recommend_sys_temp1_openurl(it.detail_url)">
                <img :src="it.cover||''" :onerror="default_img">
                <span class="bottom-txt" :title="it.title">{{it.title}}</span>
            </div>
        </div>
    </div>
</div>
</div>`;

  var list = document.getElementsByClassName('literature_recommend_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'literature_recommend_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var literature_recommend_sys_temp1_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        literature_recommend_sys_temp1_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: literature_recommend_sys_temp1_html,
        data() {
          return {
            request_of:true,//请求中
            literature_recommend_sys_temp1_margin_left: 0,//滚动坐标
            literature_recommend_sys_temp1_menu_num: 0,//当前菜单
            literature_recommend_sys_temp1_menu_list: [],//菜单列表加数据
            literature_recommend_sys_temp1_list: [],//当前列表数据
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            default_img: 'this.src="'+window.localStorage.getItem('fileUrl')+'/public/image/img-error-174x241.jpg"',
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          if (literature_recommend_sys_temp1_set_list) {
            if (literature_recommend_sys_temp1_set_list.length > 0) {
              var list = [];
              literature_recommend_sys_temp1_set_list.forEach((it, i) => {
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
              this.literature_recommend_sys_temp1_initData(list);
            }
          }
        },
        methods: {
          menuClick(val) {
            this.literature_recommend_sys_temp1_menu_num = val;
            this.literature_recommend_sys_temp1_margin_left = 0;
            this.literature_recommend_sys_temp1_menu_list = this.literature_recommend_sys_temp1_list[val]['titleInfos'] || [];
          },
          literature_recommend_sys_temp1_openurl(url) {
            window.open(url)
          },
          literature_recommend_sys_temp1_initData(list) {
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
                this.literature_recommend_sys_temp1_list = res.data.data || [];
                if (res.data.data.length > 0) {
                  this.literature_recommend_sys_temp1_menu_list = res.data.data[0].titleInfos || [];
                  this.literature_recommend_sys_temp1_intervalTime();
                }

              }
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
          literature_recommend_sys_temp1_intervalTime() {
            // console.log('启动定时');
            var _this = this;
            setInterval(() => {
              _this.literature_recommend_sys_temp1_nextClick();
            }, 3000);
          },
          //下一列封面
          literature_recommend_sys_temp1_nextClick() {
            var count_num = this.literature_recommend_sys_temp1_menu_list.length;
            var cu_width = (this.$refs.literature_recommend_sys_temp1_ref.clientWidth - ((count_num - 1) * 26)) + 40;
            var cu_num = Math.floor(cu_width / 146);

            if (this.literature_recommend_sys_temp1_margin_left == (count_num - cu_num - 1) * 146 || this.literature_recommend_sys_temp1_margin_left > (count_num - cu_num - 1) * 146) {
              this.literature_recommend_sys_temp1_margin_left = 0;
            } else {
              this.literature_recommend_sys_temp1_margin_left = this.literature_recommend_sys_temp1_margin_left + 146;
            }
            // console.log('总数：'+count_num,'要移动的宽度：'+cu_width,cu_num,this.literature_recommend_sys_temp1_margin_left);
          },
          //上一列封面
          literature_recommend_sys_temp1_preClick() {
            if (this.literature_recommend_sys_temp1_margin_left != 0) {
              this.literature_recommend_sys_temp1_margin_left = this.literature_recommend_sys_temp1_margin_left - 146;
            }
          },
        },
      });
    }
  }
}
literature_recommend_sys_temp1()