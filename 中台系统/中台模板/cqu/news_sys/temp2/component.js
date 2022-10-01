function cqu_news_sys_temp2() {
  var template_html = `<div class="index-warp-box">
  <div class="index-news-box">
    <div class="index-news-title">
      <div>
        <span @click="cqu_handleChange(index)" :class="index==cqu_curIndex?'index-news-title-red':''" v-for="(item,index) in cqu_list" :key="index">{{item.columnName || '无'}}</span>
      </div>
      <span class="index-news-title-more" @click="cqu_linkTo">更多+</span>
    </div>
    <div class="index-news-cont" v-if="cqu_list.length>0">
      <div class="index-news-list" v-for="(item,index) in cqu_list[cqu_curIndex].contentList" :key="index" @click="cqu_linkTo(item)">
        <span class="index-news-list-title">{{item.title}}</span>
        <span class="index-news-list-time">{{item.createTime.slice(0,10)}}</span>
      </div>
    </div>
    <div  class="index-news-cont" v-else>
      <div class="temp-loading" v-if="request_of"></div><!--加载中-->
      <div class="web-empty-data"  v-else-if="cqu_list[cqu_curIndex].contentList.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
      </div><!--暂无数据-->
    </div>
  </div>
</div>`;

  var list = document.getElementsByClassName('cqu_news_sys_temp2');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_news_sys_temp2 jl_vip_zt_vray jl_vip_zt_warp');
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
            cqu_curIndex: 0,
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
              window.open(val.externalLink );
            }else if (val.jumpLink) {
              window.open(val.jumpLink);
            }else{
              window.open(this.cqu_list[this.cqu_curIndex].jumpLink);
            }
            
          },
          cqu_handleChange(index) {
            this.cqu_curIndex = index;
          },
          cqu_initData(list) {
            var post_url = this.post_url_vip + '/news/api/news/pront-scenes-news';
            axios({
              url: post_url,
              method: 'POST',
              data: list,
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.cqu_list = res.data.data || [];
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
cqu_news_sys_temp2();