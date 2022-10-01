function cqu_news_sys_temp3() {
  var template_html = `<div class="index-warp-box"><div class="index-intelligence-box">
  <div class="index-intelligence-title">
    <div>
      <span class="index-intelligence-title-red">{{cqu_list.columnName}}</span>
      <!-- <span>INTELLIGENCE</span> -->
    </div>
    <span class="index-intelligence-title-more" @click="cqu_linkTo">更多+</span>
  </div>
  <div class="index-intelligence-cont" v-if="cqu_list&&cqu_list.childs&&cqu_list.childs.length>0">
    <div class="index-intelligence-list" v-for="(item,index) in cqu_list.childs" :key="index">
      <h5>{{item.childCatorage}}</h5>
      <div>
        <span v-for="(item1,index1) in item.contentList" :key="index1" @click="cqu_linkTo(item1)">{{item1.title}}</span>
      </div>
    </div>
  </div>
  <div class="index-intelligence-cont" v-else>
    <div class="temp-loading" v-if="request_of"></div><!--加载中-->
    <div class="web-empty-data"  v-else-if="cqu_list.childs.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div><!--暂无数据-->
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_news_sys_temp3');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_news_sys_temp3 jl_vip_zt_vray jl_vip_zt_warp');
      var template_temp_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        template_temp_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }

      new Vue({
        el: '#' + list[i].lastChild.id,
        template: template_html,
        data() {
          return {
            imgPath: window.localStorage.getItem('fileUrl')+'/public/image/',//公共图片路径
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            request_of:true,//请求中
            cqu_list: [],
          }
        },
        mounted() {
          var template_temp_data_list = [];
          if(template_temp_set_list){
            for (var i = 0; i < template_temp_set_list.length; i++) {
              var topCount = template_temp_set_list[i].topCount;
              var columnid = template_temp_set_list[i].id;
              var OrderRule = template_temp_set_list[i].sortType;
              template_temp_data_list.push({ count: topCount, columnId: columnid, sortField: OrderRule });
            }
          }
          this.cqu_initData(template_temp_data_list);
        },
        methods: {
          cqu_linkTo(val) {
            if (val.externalLink && val.externalLink != '' && val.externalLink != undefined) {
              window.open(val.externalLink);
            }else if (val.jumpLink) {
              window.open(val.jumpLink);
            }else{
              window.open(this.cqu_list.jumpLink);
            }
          },
          cqu_initData(list) {
            var post_url = this.post_url_vip + '/news/api/news/pront-scenes-news-cqu';
            axios({
              url: post_url,
              method: 'POST',
              data:list,
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.cqu_list = res.data.data[0] || [];
                this.request_of = false;
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
cqu_news_sys_temp3()