function literature_project_sys_temp1() {
  var literature_project_sys_temp1_html = `<div class="temp-special1-warp">
<div class="main-title-temp"><span>专题馆</span><span class="e-title"></span></div>
<div class="special1-banner-warp" ref="literature_project_sys_temp1_ref">
    <div class="special1-b-pre" @click="literature_project_sys_temp1_preClick()"></div>
    <div class="special1-b-next" @click="literature_project_sys_temp1_nextClick()"></div>
    <div class="s-b-warp" :style="{'margin-left':-literature_project_sys_temp1_margin_left+'px'}">
        <div class="box" v-for="(it,i) in literature_project_sys_temp1_list" :title="it.assemblyName">
            <img :src="fileUrl+it.cover" @click="literature_project_sys_temp1_openurl(it.jumpLink)">
            <span class="bottom-txt">{{it.assemblyName||'无'}}</span>
        </div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && literature_project_sys_temp1_list && literature_project_sys_temp1_list.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
    </div>
</div>
</div>`;

  var list = document.getElementsByClassName('literature_project_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') == -1) {
      list[i].setAttribute('class', 'literature_project_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var literature_project_sys_temp1_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        literature_project_sys_temp1_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: literature_project_sys_temp1_html,
        data() {
          return {
            request_of:true,//请求中
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            literature_project_sys_temp1_margin_left: 0,
            literature_project_sys_temp1_list: [],
          }
        },
        mounted() {
          if (literature_project_sys_temp1_set_list) {
            var topCount = literature_project_sys_temp1_set_list[0].topCount;
            var columnid = literature_project_sys_temp1_set_list[0].id;
            var sortType = literature_project_sys_temp1_set_list[0].sortType;
            this.literature_project_sys_temp1_initData(topCount, columnid, sortType);
          }
        },
        methods: {
          literature_project_sys_temp1_openurl(url) {
            window.open(url)
          },
          literature_project_sys_temp1_initData(topCount, columnid, sortType) {
            var post_url = this.post_url_vip+'/assembly/api/assembly/GetAssemblysInPortal';
            var list = [{ count: topCount, columnid: columnid, orderrule: sortType }];
            axios({
              url: post_url,
              data: list,
              method: 'POST',
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              this.request_of = false;
              if (res.data && res.data.statusCode == 200) {
                if (res.data.data.length > 0) {
                  this.literature_project_sys_temp1_list = res.data.data[0].list || [];
                  this.literature_project_sys_temp1_intervalTime();
                }
              }
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
          literature_project_sys_temp1_intervalTime() {
            var _this = this;
            setInterval(() => {
              _this.literature_project_sys_temp1_nextClick();
            }, 4000);
          },
          //下一列封面
          literature_project_sys_temp1_nextClick() {
            var count_num = this.literature_project_sys_temp1_list.length;
            var cu_width = this.$refs.literature_project_sys_temp1_ref.clientWidth - ((count_num - 1) * 20);
            var cu_num = Math.floor(cu_width / 400);

            if (this.literature_project_sys_temp1_margin_left == (count_num - cu_num - 1) * 400 || this.literature_project_sys_temp1_margin_left > (count_num - cu_num - 1) * 400) {
              this.literature_project_sys_temp1_margin_left = 0;
            } else {
              this.literature_project_sys_temp1_margin_left = this.literature_project_sys_temp1_margin_left + 400;
            }
            // console.log('总数：'+count_num,'要移动的宽度：'+cu_width,cu_num,this.literature_project_sys_temp1_margin_left);
          },
          //上一列封面
          literature_project_sys_temp1_preClick() {
            if (this.literature_project_sys_temp1_margin_left != 0) {
              this.literature_project_sys_temp1_margin_left = this.literature_project_sys_temp1_margin_left - 400;
            }
            // console.log(this.literature_project_sys_temp1_margin_left);
          },
        },
      });
    }
  }
}
literature_project_sys_temp1()