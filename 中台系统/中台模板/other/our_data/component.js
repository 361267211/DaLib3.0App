function other_our_data() {
    var other_our_data_html = `<div class="temp-ourdata-warp">
<div class="main-title-temp"><span>本馆数据</span><span class="e-title"></span></div>
<div class="ourdata-content">
    <div class="o-list-w c-l">
    
        <div class="ourdata-c-l">
            <div class="o-c-sub-title"><span>服务数据</span><span class="count">范围数据：近30天</span></div>
            <div class="o-list c-l">
                <div class="w-50">
                    <span v-for="(it,i) in other_our_data_list1" v-if="i<3">
                        <i class="child_color4_text_color">{{it.count}}</i>
                        <span>{{it.displayTitle}}</span>
                    </span>
                </div>
                <div class="w-50">
                    <span v-for="(it,i) in other_our_data_list1" v-if="i>2">
                        <i class="child_color4_text_color">{{it.count}}</i>
                        <span>{{it.displayTitle}}</span>
                    </span>
                </div>
                <div class="temp-loading" v-if="request_of"></div><!--加载中-->
                <div class="web-empty-data" v-if="!request_of && other_our_data_list1 && other_our_data_list1.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
                </div><!--暂无数据-->
            </div>
        </div>
        <div class="ourdata-c-r">
            <div class="o-c-sub-title"><span>资源数据</span><span class="count">范围数据：数据到昨天</span></div>
            <div class="o-list c-l">
                <div class="w-30">
                    <span v-for="(it,i) in other_our_data_list2" v-if="i<3">
                        <i class="child_color4_text_color">{{it.count}}</i>
                        <span>{{it.displayTitle}}</span>
                    </span>
                </div>
                <div class="w-30">
                    <span v-for="(it,i) in other_our_data_list2" v-if="i>2&&i<6">
                        <i class="child_color4_text_color">{{it.count}}</i>
                        <span>{{it.displayTitle}}</span>
                    </span>
                </div>
                <div class="w-30">
                    <span v-for="(it,i) in other_our_data_list2" v-if="i>5">
                        <i class="child_color4_text_color">{{it.count}}</i>
                        <span>{{it.displayTitle}}</span>
                    </span>
                </div>
                <div class="temp-loading" v-if="request_of"></div><!--加载中-->
                <div class="web-empty-data" v-if="!request_of && other_our_data_list2 && other_our_data_list2.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
                </div><!--暂无数据-->
            </div>
        </div>
    
    </div>
</div>
</div>`;


    var list = document.getElementsByClassName('other_our_data');
    for (var i = 0; i < list.length; i++) {
        if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
            list[i].setAttribute('class', 'other_our_data jl_vip_zt_vray jl_vip_zt_warp');
            new Vue({
                el: '#' + list[i].lastChild.id,
                template: other_our_data_html,
                data() {
                    return {
                        request_of:true,//请求中
                        other_our_data_list1: [],
                        other_our_data_list2: [],
                        fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
                        post_url_vip: window.apiDomainAndPort,
                    }
                },
                mounted() {
                    this.other_our_data_initDataList1();
                    this.other_our_data_initDataList2();
                },
                methods: {
                    other_our_data_initDataList1() {
                        var post_url = this.post_url_vip + '/loganalysis/api/log-analysis/display-log-analyses?Type=1&Range=1';
                        axios({
                            url: post_url,
                            method: 'GET',
                            headers: {
                                'Content-Type': 'text/plain',
                                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                            },
                        }).then(res => {

                            if (res.data && res.data.statusCode == 200) {
                                this.other_our_data_list1 = res.data.data || [];
                            }
                        }).catch(err => {
                            console.log(err);
                        });
                    },
                    other_our_data_initDataList2() {
                        var post_url = this.post_url_vip + '/loganalysis/api/log-analysis/display-log-analyses?Type=2&Range=1';
                        axios({
                            url: post_url,
                            method: 'GET',
                            headers: {
                                'Content-Type': 'text/plain',
                                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                            },
                        }).then(res => {
                            this.request_of = false;
                            if (res.data && res.data.statusCode == 200) {
                                this.other_our_data_list2 = res.data.data || [];
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
other_our_data()