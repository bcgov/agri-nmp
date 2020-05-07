# cat Dockerfile | oc new-build -D - --name=certbot
FROM registry.access.redhat.com/rhel7:7.8-265

RUN set -x && \
    yum install -y https://mirror.csclub.uwaterloo.ca/fedora/epel/epel-release-latest-7.noarch.rpm && \
    yum install --disablerepo="*" '--enablerepo=rhel-7-server-rpms' '--enablerepo=rhel-server-rhscl-7-rpms' '--enablerepo=rhel-7-server-extras-rpms' '--enablerepo=epel' '--enablerepo=rhel-7-server-optional-rpms' -y yum-utils certbot-1.3.0-1.el7 && \
    mkdir -p /var/log/letsencrypt && \
    chgrp -R 0 /var/log/letsencrypt && \
    chmod -R ugo+rwX /var/log/letsencrypt && \
    mkdir -p /var/lib/letsencrypt && \
    chgrp -R 0 /var/lib/letsencrypt && \
    chmod -R ugo+rwX /var/lib/letsencrypt && \
    mkdir -p /etc/letsencrypt && \
    chgrp -R 0 /etc/letsencrypt && \
    chmod -R ugo+rwX /etc/letsencrypt && \
    yum clean all -y

RUN set -x && \
    yum install '--disablerepo=*' '--enablerepo=rhel-7-server-rpms' '--enablerepo=rhel-server-rhscl-7-rpms' '--enablerepo=rhel-7-server-extras-rpms' '--enablerepo=epel' '--enablerepo=rhel-7-server-optional-rpms' '--enablerepo=rhel-7-server-ose-3.11-rpms' -y jq && \
    curl -sfL https://github.com/openshift/origin/releases/download/v3.11.0/openshift-origin-client-tools-v3.11.0-0cbc58b-linux-64bit.tar.gz -o /tmp/openshift-origin-client-tools.tar.gz && \
    tar -xzvf /tmp/openshift-origin-client-tools.tar.gz -C /usr/local/bin --strip-components=1 openshift-origin-client-tools-v3.11.0-0cbc58b-linux-64bit/oc && \
    rm /tmp/openshift-origin-client-tools.tar.gz && \
    yum clean all -y

COPY oc-deploy-certs.sh /usr/local/bin/oc-deploy-certs.sh

ENTRYPOINT ["/usr/local/bin/oc-deploy-certs.sh"]
