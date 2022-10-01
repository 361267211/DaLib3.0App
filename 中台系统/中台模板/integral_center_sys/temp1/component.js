function integral_center_sys_temp1() {
  var integral_center_sys_temp1_html = `<div class="tmp-detail" id="integral_center_sys_temp1_html">
<div class="title-box">
    <span class="left">积分任务</span>
    <span class="right">
      <span @click="integral_center_sys_temp1_openurlDetails(integral_center_sys_temp1_data.linkUrl+'/#/web_integralCenter')">更多 </span>
    </span>
</div>
<div class="tem-content" v-if="!request_of">
    <div class="integral-temp1-cont-left">
        <div class="integral-temp1-cont-left-top">
            <div>
                <div>我的积分</div>
                <span>{{integral_center_sys_temp1_data.score}}</span>
            </div>
            <div>
                <div>我的勋章</div>
                <span>{{integral_center_sys_temp1_data.medalCount}}</span>
            </div>
        </div>
        <div class="integral-temp1-cont-left-link">
            <span @click="integral_center_sys_temp1_openurlDetails(integral_center_sys_temp1_data.linkUrl+'/#/web_integralCenter')">积分中心</span>
            <span @click="integral_center_sys_temp1_openurlDetails(integral_center_sys_temp1_data.linkUrl+'/#/web_myIntegral')">我的积分</span>
            <span @click="integral_center_sys_temp1_openurlDetails(integral_center_sys_temp1_data.linkUrl+'/#/web_integralRank')">积分排行</span>
            <span @click="integral_center_sys_temp1_openurlDetails(integral_center_sys_temp1_data.linkUrl+'/#/web_integralTask')">积分任务</span>
        </div>
        <div class="integral-temp1-cont-left-btn" @click="integral_center_sys_temp1_openurlDetails(integral_center_sys_temp1_data.linkUrl+'/#/web_integralShop')">积分商城</div>
    </div>
    <div class="integral-temp1-cont-right">
        <div class="integral-temp1-cont-right-item":style="{'background-image': item.introPicUrl&&item.introPicUrl!=''?'url('+fileUrl+item.introPicUrl+')':''}" v-for="item in integral_center_sys_temp1_data.scoreTasks">
            <div class="integral-temp1-cont-right-item-title">{{item.taskName}}</div>
            <span class="integral-temp1-cont-right-item-score">+{{item.score}}</span>
            <div class="integral-temp1-cont-right-item-btn" @click="integral_center_sys_temp1_openurlDetails(item.link)" v-if="item.link&&item.link!=''&&item.link!='#'">去完成</div>
            <span class="integral-temp1-cont-right-item-time" v-if="item.endDate">截止日期：{{item.endDate.substring(0,10)}}</span>
        </div>
    </div>
</div>
<div class="tem-content">
  <!--加载中-->
  <div class="temp-loading" v-if="request_of"></div>
  <!--暂无数据-->
  <div class="web-empty-data"  v-else-if="integral_center_sys_temp1_data.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
  </div>
</div>
</div>`;
  //jl_vip_zt_vray 渲染过一次就不再重写渲染，以此为标记;
  //jl_vip_zt_warp 必须添加，是为了渲染里面的删除标签等样式

  var list = document.getElementsByClassName('integral_center_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'integral_center_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: integral_center_sys_temp1_html,
        data() {
          return {
            request_of:true,
            integral_center_sys_temp1_data: null,
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          this.integral_center_sys_temp1_initData();
        },
        methods: {
          //查看详情，到详情页面
          integral_center_sys_temp1_openurlDetails(url) {
            window.open(url);
          },
          integral_center_sys_temp1_initData() {
            var post_url = this.post_url_vip + '/scorecenter/api/scene/score-center-scene-data';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.integral_center_sys_temp1_data = res.data.data || {};
              }
              this.request_of = false;
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
integral_center_sys_temp1()

